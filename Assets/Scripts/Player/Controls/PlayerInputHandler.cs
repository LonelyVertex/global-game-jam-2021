using InputSystem;
using Player.Energy;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Player.Controls
{
    public class PlayerInputHandler
    {
        readonly InputActions inputActions = new InputActions();

        public Vector3 MovementVector3 { get; private set; }

        public PlayerInputHandler(GameManager gameManager)
        {
            gameManager.OnGameStart += EnablePlayerInput;
            gameManager.OnGameOver += DisablePlayerInput;
            
            DisablePlayerInput();
        }
        
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
