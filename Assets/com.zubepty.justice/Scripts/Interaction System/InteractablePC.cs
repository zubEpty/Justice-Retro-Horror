using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractablePC : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _desktopCanvas;
    [SerializeField] private GameObject _cctvCam;
    [SerializeField] private string promptName = "Desktop";


    public void Interact()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _desktopCanvas.gameObject.SetActive(true);
        _player.gameObject.SetActive(false);
      
    }

    [ContextMenu("Cancel Interaction")]
    public void CancelInteraction()
    {
        _desktopCanvas.SetActive(false);
        _player.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _cctvCam.SetActive(false);
        Debug.Log("PC interaction cancelled.");
    }

    public bool IsInteractionOngoing => true;
    public string PromptMessage => $"Press E to interact with {promptName}";
}
