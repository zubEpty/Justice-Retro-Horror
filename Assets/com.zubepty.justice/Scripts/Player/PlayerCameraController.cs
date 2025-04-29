using UnityEngine;


namespace FirstPersonController
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform playerBody;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float sensitivity = 2f;
        [SerializeField] private float maxPitch = 80f;
        [SerializeField] private float minPitch = -80f;

        private float _pitch;
        private Vector2 _lookInput;

        private void OnEnable()
        {
            PlayerInputHandler.Instance.OnLookInput.AddListener(HandleLookInput);
        }

        private void OnDisable()
        {
            PlayerInputHandler.Instance.OnLookInput.RemoveListener(HandleLookInput);
        }

        public void LockMouseCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        private void HandleLookInput(Vector2 input)
        {
            _lookInput = input;
        }

        private void Start()
        {
            LockMouseCursor();
        }

        private void Update()
        {
            LookAround();
        }

        private void LookAround()
        {
            if (_lookInput.sqrMagnitude < 0.01f)
                return;

            float mouseX = _lookInput.x * sensitivity;
            float mouseY = _lookInput.y * sensitivity;

            // Rotate player left/right (yaw)
            playerBody.Rotate(Vector3.up * mouseX);

            // Rotate camera up/down (pitch)
            _pitch -= mouseY;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

            cameraTransform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }

}
