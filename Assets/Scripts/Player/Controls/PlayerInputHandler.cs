using System;
using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controls
{
    public class PlayerInputHandler
    {
        readonly InputActions inputActions = new InputActions();

        public Vector3 MovementVector3 { get; private set; }
        public bool IsMoving => !Mathf.Approximately(MovementVector3.magnitude, 0.0f);
        public event Action OnDrillingStarted;
        public event Action OnDrillingStopped;

        public void EnablePlayerInput()
        {
            inputActions.Enable();
            EnableMovement();
            inputActions.PlayerControls.Drill.performed += DrillPerformed;
            inputActions.PlayerControls.Drill.canceled += DrillCanceled;
        }

        void EnableMovement()
        {
            inputActions.PlayerControls.Movement.performed += MovementPerformed;
            inputActions.PlayerControls.Movement.canceled += MovementCanceled;
        }

        void DisableMovement()
        {
            inputActions.PlayerControls.Movement.performed -= MovementPerformed;
            inputActions.PlayerControls.Movement.canceled -= MovementCanceled;
        }

        public void DisablePlayerInput()
        {
            DisableMovement();
            inputActions.PlayerControls.Drill.performed -= DrillPerformed;
            inputActions.PlayerControls.Drill.canceled -= DrillCanceled;
            Reset();
            inputActions.Disable();
        }

        void DrillPerformed(InputAction.CallbackContext obj)
        {
            Reset();
            DisableMovement();
            OnDrillingStarted?.Invoke();
        }

        void DrillCanceled(InputAction.CallbackContext obj)
        {
            EnableMovement();
            OnDrillingStopped?.Invoke();
        }

        void MovementCanceled(InputAction.CallbackContext obj)
        {
            Reset();
        }

        void MovementPerformed(InputAction.CallbackContext obj)
        {
            var inputVector = obj.ReadValue<Vector2>();
            MovementVector3 = new Vector3(inputVector.x, 0, inputVector.y);
        }

        void Reset()
        {
            MovementVector3 = Vector3.zero;
        }
    }
}
