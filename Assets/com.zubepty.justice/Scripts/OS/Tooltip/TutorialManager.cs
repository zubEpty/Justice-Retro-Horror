using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> tutorialSteps;
    public TextMeshProUGUI tutorialTextUI;
    public Button nextButton;
    private Button lastStepButton;

    private int currentStepIndex = 0;
    private bool isConditionMet = false;
    private bool stepInProgress = false;


    public UnityEvent OnTutorialCompleted;

    void Start()
    {
        ShowStep(currentStepIndex);
    }

    void ShowStep(int index)
    {
        isConditionMet = false;
        nextButton.onClick.RemoveAllListeners();

        if (lastStepButton != null)
        {
            lastStepButton.onClick.RemoveAllListeners();
            lastStepButton = null;
        }


        if (index >= tutorialSteps.Count)
        {
            tutorialTextUI.text = "Tutorial Complete!";
            OnTutorialCompleted?.Invoke();
            nextButton.gameObject.SetActive(false);
            return;
        }

        var step = tutorialSteps[index];
        tutorialTextUI.text = step.tutorialText;

        switch (step.conditionType)
        {
            case TutorialConditionType.ManualNext:
                nextButton.gameObject.SetActive(true);
                nextButton.interactable = true;
                nextButton.onClick.AddListener(GoToNextStep);
                break;

            case TutorialConditionType.ButtonClick:
                nextButton.gameObject.SetActive(false);
                var targetBtn = GameObject.Find(step.requiredButtonName)?.GetComponent<Button>();
                if (targetBtn != null)
                {
                    lastStepButton = targetBtn;
                    targetBtn.onClick.AddListener(() => {
                        if (!isConditionMet)
                        {
                            isConditionMet = true;
                            GoToNextStep();
                        }
                    });
                }
                break;

            case TutorialConditionType.ConditionMet:
                nextButton.gameObject.SetActive(false);
                StartCoroutine(WaitForCondition());
                break;
        }
    }

    IEnumerator WaitForCondition()
    {
        yield return new WaitUntil(() => isConditionMet);
        GoToNextStep();
    }

    public void GoToNextStep()
    {
        if (stepInProgress) return;
        stepInProgress = true;

        currentStepIndex++;
        ShowStep(currentStepIndex);

        // Allow next step after frame delay
        StartCoroutine(UnlockNextStep());
    }

    IEnumerator UnlockNextStep()
    {
        yield return null; // wait one frame
        stepInProgress = false;
    }

    public void SetConditionMetExternally()
    {
        isConditionMet = true;
    }
}
