using UnityEngine;

public class LockerHideManager : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private GameObject _lockerCamera;
    [SerializeField] private CharacterController _playerController;
    [SerializeField] private GameObject _player;


    public bool IsInteractionOngoing => true;

    public string PromptMessage => $"Press E to Hide Locker";

    public void CancelInteraction()
    {
        _playerCamera.SetActive(true);
        _lockerCamera.SetActive(false);
        _playerController.enabled = true;
        _player.gameObject.transform.position -= new Vector3(0, 0.4f, 0);

    }

    public void Interact()
    {
        _playerCamera.SetActive(false);
        _lockerCamera.SetActive(true);
        _playerController.enabled = false;
        _player.gameObject.transform.position += new Vector3(0, 0.4f, 0);
    }

   
}
