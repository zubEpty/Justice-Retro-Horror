using FirstPersonController;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [SerializeField] private Light flashlight; // Assign in inspector
    [SerializeField] private bool flashlightUnlocked = false;
    private bool isOn = false;

    private void OnEnable()
    {
        PlayerInputHandler.Instance.OnToggleFlashlightInput.AddListener(ToggleFlashlight);
    }

    private void OnDisable()
    {
        PlayerInputHandler.Instance.OnToggleFlashlightInput.RemoveListener(ToggleFlashlight);
    }

    private void ToggleFlashlight()
    {
        if (!flashlightUnlocked) return;

        isOn = !isOn;
        flashlight.enabled = isOn;
    }

    // Call this from elsewhere to unlock flashlight ability
    public void UnlockFlashlight()
    {
        flashlightUnlocked = true;
    }
}
