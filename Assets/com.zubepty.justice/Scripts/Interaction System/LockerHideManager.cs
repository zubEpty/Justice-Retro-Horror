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
        _player.gameObject.transform.position = new Vector3(_player.gameObject.transform.position.x, 2.1f, _player.gameObject.transform.position.z);

    }

    public void Interact()
    {
        _playerCamera.SetActive(false);
        _lockerCamera.SetActive(true);
        _playerController.enabled = false;

        _player.gameObject.transform.position = new Vector3(_player.gameObject.transform.position.x, 2.4f, _player.gameObject.transform.position.z);
    }

   
}
