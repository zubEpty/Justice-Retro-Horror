using UnityEngine;

namespace FirstPersonController
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private LayerMask interactableLayers;

        [SerializeField] private Camera _camera;

   
        private void OnEnable()
        {
            PlayerInputHandler.Instance.OnInteractInput.AddListener(HandleInteract);
        }

        private void OnDisable()
        {
            PlayerInputHandler.Instance.OnInteractInput.RemoveListener(HandleInteract);
        }

        private void HandleInteract()
        {
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayers))
            {
                Debug.Log($"Interacted with {hit.collider.name}");
            }
            else
            {
                Debug.Log("No interactable object found.");
            }
        }
    }
}
