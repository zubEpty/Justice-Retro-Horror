using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractablePC : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _player;

    public void Interact()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _player.gameObject.SetActive(false);
        SceneManager.LoadScene("OS", LoadSceneMode.Additive);
    }
}
