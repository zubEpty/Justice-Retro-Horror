using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Light targetLight;
    [SerializeField] private string promptName = "Switch";

    public void Interact()
    {
        if (targetLight != null)
        {
            targetLight.enabled = !targetLight.enabled;
            Debug.Log("Light toggled.");
        }
    }

    public void CancelInteraction()
    {

    }

    public bool IsInteractionOngoing => false;
    public string PromptMessage => $"Press E to interact with {promptName}";
}