using System.Collections;
using System.Collections.Generic;
using FirstPersonController;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerText;
    public float typingSpeed = 0.05f;

    public GameObject DialogueCanvas;

    public DialogueSet currentDialogue;
    public DialogueSet startSequence;
    private int currentIndex = 0;
    private Coroutine typingCoroutine;

    public UnityEvent OnDialogueEnded;

    private Dictionary<string, string> conditions = new();

    private void Start()
    {
        StartDialogue(startSequence);
    }


    public void StartDialogue(DialogueSet dialogue)
    {
        DialogueCanvas.SetActive(true);
        currentDialogue = dialogue;
        currentIndex = 0;
        ShowLine();
    }


    void OnEnable()
    {
        if (PlayerInputHandler.Instance != null)
            PlayerInputHandler.Instance.OnAdvanceDialogueInput.AddListener(NextLine);
    }

    void OnDisable()
    {
        if (PlayerInputHandler.Instance != null)
            PlayerInputHandler.Instance.OnAdvanceDialogueInput.RemoveListener(NextLine);
    }


    public void NextLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentDialogue.lines[currentIndex].text;
            typingCoroutine = null;
            return;
        }

        currentIndex++;
        if (currentIndex < currentDialogue.lines.Count)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowLine()
    {
        var line = currentDialogue.lines[currentIndex];

        if (!string.IsNullOrEmpty(line.conditionKey))
        {
            if (!conditions.TryGetValue(line.conditionKey, out var val) || val != line.conditionValue)
            {
                NextLine(); // Skip this line if condition fails
                return;
            }
        }

        speakerText.text = line.speaker;
        typingCoroutine = StartCoroutine(TypeText(line.text));
    }

    IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

    void EndDialogue()
    {
        dialogueText.text = "";
        speakerText.text = "";
        DialogueCanvas.SetActive(false);
        Debug.Log("Dialogue Ended.");
        OnDialogueEnded?.Invoke();
    }

    public void SetCondition(string key, string value) => conditions[key] = value;
    public void RemoveCondition(string key) => conditions.Remove(key);
}
