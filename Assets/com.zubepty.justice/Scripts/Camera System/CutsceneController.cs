using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _cutsceneCamera;
    [SerializeField] private GameObject _titleText;

    private void Start()
    {
        PlayCutscene();
    }


    public void PlayCutscene()
    {
        _director.Play();
    }

    public void LoadPlayerScequence()
    {
        _player.SetActive(true);
        _cutsceneCamera.SetActive(false);
        _titleText.SetActive(false);
    }

}
