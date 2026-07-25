using UnityEngine;
using UnityEngine.UI;

public class EmailContentDownloadController : MonoBehaviour
{
    [SerializeField] private Button downloadButton;
    [SerializeField] private AntivirusDownloadController downloadController;

    private void OnEnable()
    {
        ResolveReferences();

        if (downloadButton == null)
            return;

        downloadButton.interactable = true;
        downloadButton.onClick.RemoveListener(StartDownload);
        downloadButton.onClick.AddListener(StartDownload);
    }

    private void OnDisable()
    {
        if (downloadButton != null)
            downloadButton.onClick.RemoveListener(StartDownload);
    }

    private void StartDownload()
    {
        if (downloadController != null)
            downloadController.StartDownload();
    }

    private void ResolveReferences()
    {
        if (downloadButton == null)
            downloadButton = FindChildButton("Btn_Virus_Download");

        if (downloadButton == null)
            downloadButton = GetComponentInChildren<Button>(true);

        if (downloadController == null)
            downloadController = FindSceneComponent<AntivirusDownloadController>();
    }

    private Button FindChildButton(string objectName)
    {
        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            if (button.name == objectName)
                return button;
        }

        return null;
    }

    private static T FindSceneComponent<T>() where T : Component
    {
        T[] components = Resources.FindObjectsOfTypeAll<T>();
        foreach (T component in components)
        {
            if (component.gameObject.scene.IsValid())
                return component;
        }

        return null;
    }
}
