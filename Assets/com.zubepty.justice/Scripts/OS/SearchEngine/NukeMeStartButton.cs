using UnityEngine;
using UnityEngine.UI;

public class NukeMeStartButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        if (_button != null)
        {
            _button.onClick.RemoveListener(StartGame);
            _button.onClick.AddListener(StartGame);
        }
    }

    private void OnDestroy()
    {
        if (_button != null)
            _button.onClick.RemoveListener(StartGame);
    }

    public void StartGame()
    {
        BegulaVirusFlowController flowController = FindFlowController();
        if (flowController != null)
        {
            flowController.ResetOwnedPcAndStartGame();
            return;
        }

        GameObject mainMenu = FindSceneObject("Main_menu");
        if (mainMenu != null)
            mainMenu.SetActive(false);
    }

    private static BegulaVirusFlowController FindFlowController()
    {
        BegulaVirusFlowController[] controllers = Resources.FindObjectsOfTypeAll<BegulaVirusFlowController>();
        foreach (BegulaVirusFlowController controller in controllers)
        {
            if (controller != null && controller.gameObject.scene.IsValid())
                return controller;
        }

        return null;
    }

    private static GameObject FindSceneObject(string objectName)
    {
        GameObject[] sceneObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject sceneObject in sceneObjects)
        {
            if (sceneObject.name == objectName && sceneObject.scene.IsValid())
                return sceneObject;
        }

        return null;
    }
}
