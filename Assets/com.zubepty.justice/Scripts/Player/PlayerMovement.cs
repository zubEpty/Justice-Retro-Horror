using UnityEngine;

namespace FirstPersonController
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private Transform _cameraTransform;

        private CharacterController _characterController;
        private Vector2 _moveInput;
      
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
    
        }

        private void OnEnable()
        {
            PlayerInputHandler.Instance.OnMoveInput.AddListener(HandleMoveInput);
        }

        private void OnDisable()
        {
            PlayerInputHandler.Instance.OnMoveInput.RemoveListener(HandleMoveInput);
        }

        private void Update()
        {
            Move();
        }

        private void HandleMoveInput(Vector2 input)
        {
            _moveInput = input;
        }

        private void Move()
        {
            if (_moveInput.sqrMagnitude < 0.01f) return;

            Vector3 forward = _cameraTransform.forward;
            Vector3 right = _cameraTransform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 move = forward * _moveInput.y + right * _moveInput.x;
            _characterController.Move(move * moveSpeed * Time.deltaTime);
        }
    }
}
