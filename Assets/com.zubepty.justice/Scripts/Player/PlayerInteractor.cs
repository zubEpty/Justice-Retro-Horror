using UnityEngine;

namespace FirstPersonController
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private LayerMask interactableLayers;
        [SerializeField] private Camera _camera;
        [SerializeField] private InteractionPromptUI _interactionPromptUI;

        private IInteractable _currentInteractable;

        private void OnEnable()
        {
            PlayerInputHandler.Instance.OnInteractInput.AddListener(HandleInteract);
        }

        private void OnDisable()
        {
            PlayerInputHandler.Instance.OnInteractInput.RemoveListener(HandleInteract);
        }

        private void Update()
        {
            CheckForInteractable();
        }

        private void CheckForInteractable()
        {
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayers))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    _currentInteractable = interactable;
                    _interactionPromptUI.Show("Press E to Interact");
                    return;
                }
            }

            _currentInteractable = null;
            _interactionPromptUI.Hide();
        }

        private void HandleInteract()
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.Interact();
            }
        }
    }
}