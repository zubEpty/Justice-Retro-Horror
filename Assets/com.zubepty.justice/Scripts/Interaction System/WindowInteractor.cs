using DG.Tweening;
using UnityEngine;

public class WindowInteractor : MonoBehaviour, IInteractable
{
    [Header("Rotation Settings")]
    [SerializeField] private float openYRotation = 90f;
    [SerializeField] private float closedYRotation = 0f;
    [SerializeField] private float rotateDuration = 1f;
    [SerializeField] private string promptName = "Window";

    [Header("State")]
    [SerializeField] private bool isOpen = false;
    public bool IsOpen => isOpen; // Public getter

    private Tween _rotationTween;

    public void Interact()
    {
        ToggleWindow();
    }

    public void CancelInteraction()
    {
        // Not needed for window, but required by interface
    }

    public bool IsInteractionOngoing => false;
    public string PromptMessage => $"Press E to interact with {promptName}";

    public void ToggleWindow()
    {
        if (_rotationTween != null && _rotationTween.IsActive())
            _rotationTween.Kill();

        float targetY = isOpen ? closedYRotation : openYRotation;
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, targetY, transform.eulerAngles.z);

        _rotationTween = transform.DORotateQuaternion(targetRotation, rotateDuration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => isOpen = !isOpen);
    }

    /// Call from other scripts to open programmatically
    public void OpenWindow()
    {
        if (isOpen) return;
        ToggleWindow();
    }

    /// Call from other scripts to close programmatically
    public void CloseWindow()
    {
        if (!isOpen) return;
        ToggleWindow();
    }
}
