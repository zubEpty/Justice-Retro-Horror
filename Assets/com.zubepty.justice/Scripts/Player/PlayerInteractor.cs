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
        private bool _isInteracting = false;

        private void OnEnable()
        {
            PlayerInputHandler.Instance.OnInteractInput.AddListener(HandleInteract);
            PlayerInputHandler.Instance.OnCancelInteractInput.AddListener(HandleCancel);
        }

        private void OnDisable()
        {
            PlayerInputHandler.Instance.OnInteractInput.RemoveListener(HandleInteract);
            PlayerInputHandler.Instance.OnCancelInteractInput.RemoveListener(HandleCancel);
        }

        private void Update()
        {
            if (!_isInteracting)
            {
                CheckForInteractable();
            }
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
                    _interactionPromptUI.Show(interactable.PromptMessage);
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

                if (_currentInteractable.IsInteractionOngoing)
                {
                    _isInteracting = true;
                    _interactionPromptUI.Hide();
                }
            }
        }

        public void HandleCancel()
        {
            if (_isInteracting && _currentInteractable != null)
            {
                _currentInteractable.CancelInteraction();
                _isInteracting = false;
            }
        }
    }
}