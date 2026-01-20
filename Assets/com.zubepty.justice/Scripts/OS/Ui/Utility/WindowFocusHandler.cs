using UnityEngine;
using UnityEngine.EventSystems;

public class WindowFocusHandler : MonoBehaviour, IPointerDownHandler
{
    public void OpenWindow(GameObject window)
    {
        window.SetActive(true);
        window.transform.SetAsLastSibling();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }
}
