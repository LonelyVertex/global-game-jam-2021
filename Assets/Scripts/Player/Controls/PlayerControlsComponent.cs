using Player.State;
using Player.Energy;
using UnityEngine;
using Zenject;

namespace Player.Controls
{
    public class PlayerControlsComponent : MonoBehaviour
    {
        [Inject]
        PlayerInputHandler playerInputHandler;

        [Inject]
        EnergyHandler energyHandler;
        
        [Inject]
        PlayerStateComponent playerState;

        [SerializeField]
        CharacterController characterController;

        [SerializeField]
        float movementSpeedConstant = 30;

        [SerializeField]
        float dashSpeed = 60;

        [SerializeField]
        float rotationSpeed = 540;

        Quaternion nextRotation;
        bool isDashing = false;

        void OnEnable()
        {
            playerInputHandler.EnablePlayerInput();
            playerInputHandler.OnDrillingStarted += DrillStarted;
            playerInputHandler.OnDrillingStopped += DrillStopped;
            playerInputHandler.OnDashPerformed += DashPerformed;
        }

        void OnDisable()
        {
            playerInputHandler.DisablePlayerInput();
            playerInputHandler.OnDrillingStarted -= DrillStarted;
            playerInputHandler.OnDrillingStopped -= DrillStopped;
            playerInputHandler.OnDashPerformed -= DashPerformed;
        }

        void DrillStarted()
        {
            playerState.IsDrilling = true;
        }

        void DrillStopped()
        {
            playerState.IsDrilling = false;
        }

        void DashPerformed()
        {
            isDashing = true;
            energyHandler.DrainDashEnergy();
            Invoke(nameof(CancelDash), 0.1f);
        }

        void Update()
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, nextRotation, rotationSpeed * Time.deltaTime);
        }

        void FixedUpdate()
        {
            var (speed, vector) = isDashing
                ? (dashSpeed, transform.forward.normalized)
                : (movementSpeedConstant, playerInputHandler.MovementVector3);

            characterController.SimpleMove(speed * vector);

            if (playerInputHandler.IsMoving) {
                nextRotation = Quaternion.LookRotation(playerInputHandler.MovementVector3, Vector3.up);
            }
        }

        void CancelDash()
        {
            isDashing = false;
        }
    }
}
