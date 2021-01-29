using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controls
{
    public class PlayerInputHandler
    {
        readonly InputActions inputActions = new InputActions();

        public Vector3 MovementVector3 { get; private set; }


        public void EnablePlayerInput()
        {
            inputActions.Enable();
            inputActions.PlayerControls.Movement.performed += MovementPerformed;
            inputActions.PlayerControls.Movement.canceled += MovementCanceled;
        }

        public void DisablePlayerInput()
        {
            inputActions.PlayerControls.Movement.performed -= MovementPerformed;
            inputActions.PlayerControls.Movement.canceled -= MovementCanceled;
            Reset();
            inputActions.Disable();
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
