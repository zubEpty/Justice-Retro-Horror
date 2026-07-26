using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VirusDownloadButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AntivirusDownloadController downloadController;
    [SerializeField] private GameObject downloadTarget;

    public void OnPointerClick(PointerEventData eventData)
    {
        Button button = GetComponent<Button>();
        if (button != null && !button.interactable)
            return;

        if (downloadController == null)
            downloadController = FindSceneComponent<AntivirusDownloadController>();

        if (downloadTarget == null)
            downloadTarget = FindSceneObject("Btn_Virus");

        RetroAudioManager.StopDesktopMode();

        if (downloadController != null)
            downloadController.StartDownload(downloadTarget);
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
}
