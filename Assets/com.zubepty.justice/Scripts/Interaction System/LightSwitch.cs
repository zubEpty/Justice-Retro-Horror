using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Light targetLight;

    public void Interact()
    {
        if (targetLight != null)
        {
            targetLight.enabled = !targetLight.enabled;
            Debug.Log("Light toggled.");
        }
    }
}