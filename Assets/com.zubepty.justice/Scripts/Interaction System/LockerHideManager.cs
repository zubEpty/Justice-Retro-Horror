using UnityEngine;

public class LockerHideManager : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _lockerCamera;
    [SerializeField] private CharacterController _playerController;


    public bool IsInteractionOngoing => true;

    public string PromptMessage => $"Press E to Hide Locker";

    public void CancelInteraction()
    {
        _player.SetActive(true);
        _lockerCamera.SetActive(false);
        _playerController.enabled = true;
    }

    public void Interact()
    {
       _player.SetActive(false);
        _lockerCamera.SetActive(true);
        _playerController.enabled = false;
    }

   
}
