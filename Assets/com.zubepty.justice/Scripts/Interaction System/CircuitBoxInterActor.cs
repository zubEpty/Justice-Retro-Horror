using UnityEngine;

public class CircuitBoxInterActor : MonoBehaviour, IInteractable
{
    public bool IsInteractionOngoing => false;

    public string PromptMessage => "Press E to turn on the power";

    public void CancelInteraction()
    {
        
    }

    public void Interact()
    {
        GetComponent<Outline>().enabled = false;
        GameManager.Instance.LightManager_.ToggleRoomPower();       
    }
}
