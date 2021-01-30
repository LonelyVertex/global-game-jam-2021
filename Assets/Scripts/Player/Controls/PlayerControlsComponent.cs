using Player.State;
using UnityEngine;
using Zenject;

namespace Player.Controls
{
    public class PlayerControlsComponent : MonoBehaviour
    {
        [Inject]
        PlayerInputHandler playerInputHandler;

        [Inject]
        PlayerStateComponent playerState;

        [SerializeField]
        CharacterController characterController;

        [SerializeField]
        float movementSpeedConstant = 30;

        [SerializeField]
        float maxSpeed = 3;

        [SerializeField]
        float rotationSpeed = 540;

        Quaternion nextRotation;

        void OnEnable()
        {
            playerInputHandler.EnablePlayerInput();
            playerInputHandler.OnDrillingStarted += DrillStarted;
            playerInputHandler.OnDrillingStopped += DrillStopped;
        }

        void OnDisable()
        {
            playerInputHandler.DisablePlayerInput();
            playerInputHandler.OnDrillingStarted -= DrillStarted;
            playerInputHandler.OnDrillingStopped -= DrillStopped;
        }

        void DrillStarted()
        {
            playerState.IsDrilling = true;
        }

        void DrillStopped()
        {
            playerState.IsDrilling = false;
        }

        void Update()
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, nextRotation, rotationSpeed * Time.deltaTime);
        }

        void FixedUpdate()
        {
            characterController.SimpleMove(movementSpeedConstant * playerInputHandler.MovementVector3);

            if (playerInputHandler.IsMoving) {
                nextRotation = Quaternion.LookRotation(playerInputHandler.MovementVector3, Vector3.up);
            }
        }
    }
}
