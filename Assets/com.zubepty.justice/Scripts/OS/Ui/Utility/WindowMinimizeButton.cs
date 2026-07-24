using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class WindowMinimizeButton : MonoBehaviour
{
    private GameObject window;

    private void Awake()
    {
        WindowFocusHandler focusHandler = GetComponentInParent<WindowFocusHandler>();
        window = focusHandler != null ? focusHandler.gameObject : transform.parent.gameObject;
        GetComponent<Button>().onClick.AddListener(Minimize);
    }

    private void OnEnable()
    {
        if (window != null && MinimizedProgramManager.Instance != null)
            MinimizedProgramManager.Instance.NotifyWindowOpened(window);
    }

    private void Minimize()
    {
        if (window != null && MinimizedProgramManager.Instance != null)
            MinimizedProgramManager.Instance.Minimize(window);
    }
}
