using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VirusDownloadButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AntivirusDownloadController downloadController;

    public void OnPointerClick(PointerEventData eventData)
    {
        Button button = GetComponent<Button>();
        if (button != null && !button.interactable)
            return;

        if (downloadController == null)
            downloadController = FindSceneComponent<AntivirusDownloadController>();

        if (downloadController != null)
            downloadController.StartDownload();
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
