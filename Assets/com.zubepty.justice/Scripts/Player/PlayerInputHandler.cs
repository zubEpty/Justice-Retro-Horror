using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FirstPersonController
{
    [DefaultExecutionOrder(-100)]
    public class PlayerInputHandler : MonoBehaviour
    {
        public static PlayerInputHandler Instance { get; private set; }

        [Header("Input Events")]
        public UnityEvent<Vector2> OnMoveInput;
        public UnityEvent OnInteractInput;
        public UnityEvent OnCancelInteractInput;
        public UnityEvent<Vector2> OnLookInput;
        public UnityEvent OnAdvanceDialogueInput;


        private Vector2 _moveInput;
        private Vector2 _lookInput;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        // These are PUBLIC, to be called by Unity's PlayerInput component
        public void GetMoveInput(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            OnMoveInput?.Invoke(_moveInput);
        }

        public void GetAdvanceDialogueInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAdvanceDialogueInput?.Invoke();
            }
        }

        public void GetLookInput(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
            OnLookInput?.Invoke(_lookInput);
        }

        public Vector2 GetCurrentLookInput()
        {
            return _lookInput;
        }


        public void GetInteractInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnInteractInput?.Invoke();
            }
        }


        public void OnCancelInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnCancelInteractInput?.Invoke();
            }
        }


        
        public Vector2 GetCurrentMoveInput()
        {
            return _moveInput;
        }
    }
}
