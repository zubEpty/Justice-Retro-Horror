using UnityEngine;
using UnityEngine.Events;

public class CircuitBoxInterActor : MonoBehaviour, IInteractable
{

    public bool OnPowerCircuitEnabled = false;

    public bool IsInteractionOngoing => false;

    public string PromptMessage => "Press E to turn on the power";

    public UnityEvent OnCircuitBoxEnabled;

    public void CancelInteraction()
    {
        
    }

    public void Interact()
    {
        OnCircuitBoxEnabled?.Invoke();
        GetComponent<Outline>().enabled = false;
        OnPowerCircuitEnabled = true;
        GameManager.Instance.LightManager_.ToggleRoomPower();       
    }
}
