using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _gameoverPanel;

    
    void Awake()
    {
        // Singleton pattern to ensure only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
           // Destroy(gameObject);
        }
    }

    public void LoadGameOverUi()
    {
        _gameoverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public LightManager LightManager_;

  
    public void ShutDownPowerSupply()
    {
        LightManager_.ToggleRoomPower();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
 
}