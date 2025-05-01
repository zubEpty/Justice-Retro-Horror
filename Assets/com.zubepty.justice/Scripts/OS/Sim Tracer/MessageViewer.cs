using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageViewer : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text fullMessageText;
    public Button downloadEvidenceButton;

    private EvidenceSpawner evidenceSpawner;
    private string currentReferenceID;
    [SerializeField] private SimTracerFoundStep _step;

    public void Initialize(EvidenceSpawner spawner)
    {
        evidenceSpawner = spawner;
    }

    public void ShowMessage(MessageData message)
    {
        fullMessageText.text = message.content;
        panel.SetActive(true);
        currentReferenceID = message.referenceID;

        _step.SetEvidenceTracedStep(message);

        // Enable or disable download button
        bool hasEvidence = !string.IsNullOrEmpty(currentReferenceID);
        downloadEvidenceButton.gameObject.SetActive(hasEvidence);

        // Clear previous listener
        downloadEvidenceButton.onClick.RemoveAllListeners();
        if (hasEvidence)
        {
            downloadEvidenceButton.onClick.AddListener(() =>
            {
                evidenceSpawner.SpawnEvidence(currentReferenceID);
            });
        }
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
