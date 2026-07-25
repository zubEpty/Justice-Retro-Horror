using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AntivirusDownloadController : MonoBehaviour
{
    [SerializeField] private Button downloadButton;
    [SerializeField] private GameObject downloadPanel;
    [SerializeField] private Image progressFill;
    [SerializeField] private Button okButton;
    [SerializeField] private TextMeshProUGUI okButtonText;
    [SerializeField] private GameObject antivirusButton;
    [SerializeField] private float downloadDuration = 6f;
    [SerializeField] private string waitingText = "Please wait...";
    [SerializeField] private string doneText = "OK";

    private bool downloadStarted;
    private bool downloadCompleted;
    private Coroutine downloadRoutine;

    private void Awake()
    {
        ResolveReferences();

        if (downloadButton != null)
            downloadButton.onClick.AddListener(StartDownload);

        if (okButton != null)
            okButton.onClick.AddListener(CloseDownloadPanel);

        if (downloadPanel != null)
            downloadPanel.SetActive(false);

        if (progressFill != null)
            progressFill.fillAmount = 0f;

        SetOkButtonState(false, waitingText);

        if (antivirusButton != null)
            antivirusButton.SetActive(false);
    }

    public void StartDownload()
    {
        if (downloadStarted || downloadCompleted)
            return;

        downloadStarted = true;

        if (downloadButton != null)
            downloadButton.interactable = false;

        if (downloadPanel != null)
        {
            downloadPanel.SetActive(true);
            downloadPanel.transform.SetAsLastSibling();
            RaiseParentsToFront(downloadPanel.transform);
        }

        if (progressFill != null)
            progressFill.fillAmount = 0f;

        SetOkButtonState(false, waitingText);

        if (downloadRoutine != null)
            StopCoroutine(downloadRoutine);

        downloadRoutine = StartCoroutine(DownloadRoutine());
    }

    private IEnumerator DownloadRoutine()
    {
        float elapsed = 0f;

        while (elapsed < downloadDuration)
        {
            elapsed += Time.deltaTime;

            if (progressFill != null)
                progressFill.fillAmount = Mathf.Clamp01(elapsed / downloadDuration);

            yield return null;
        }

        if (progressFill != null)
            progressFill.fillAmount = 1f;

        downloadCompleted = true;
        downloadRoutine = null;

        if (antivirusButton != null)
        {
            antivirusButton.SetActive(true);
            antivirusButton.transform.SetAsLastSibling();
            RaiseParentsToFront(antivirusButton.transform);
        }

        SetOkButtonState(true, doneText);
    }

    private void CloseDownloadPanel()
    {
        if (!downloadCompleted || downloadPanel == null)
            return;

        downloadPanel.SetActive(false);
    }

    private void SetOkButtonState(bool isInteractable, string label)
    {
        if (okButton != null)
            okButton.interactable = isInteractable;

        if (okButtonText != null)
            okButtonText.text = label;
    }

    private void ResolveReferences()
    {
        if (antivirusButton == null)
            antivirusButton = FindSceneObject("Btn_Virus");

        if (antivirusButton == null)
            antivirusButton = FindSceneObject("Btn_Antivirus");
    }

    private static GameObject FindSceneObject(string objectName)
    {
        GameObject[] sceneObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject sceneObject in sceneObjects)
        {
            if (sceneObject.name == objectName && sceneObject.scene.IsValid())
                return sceneObject;
        }

        return null;
    }

    private static void RaiseParentsToFront(Transform child)
    {
        Transform current = child == null ? null : child.parent;
        while (current != null)
        {
            current.SetAsLastSibling();
            current = current.parent;
        }
    }
}
