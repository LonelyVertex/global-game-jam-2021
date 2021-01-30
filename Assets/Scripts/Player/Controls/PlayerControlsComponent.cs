using UnityEngine;
using Zenject;

namespace Player.Controls
{
    public class PlayerControlsComponent : MonoBehaviour
    {
        [Inject]
        PlayerInputHandler playerInputHandler;
        [Inject]
        GameManager gameManager;

        [SerializeField]
        Rigidbody rigidbody;

        [SerializeField]
        float movementSpeedConstant = 30;

        [SerializeField]
        float maxSpeed = 3;

        [SerializeField]
        float rotationSpeed = 540;

        Quaternion nextRotation;
        bool isDrilling;

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
            gameManager.SetPlayerDrilling(true);
        }

        void DrillStopped()
        {
            gameManager.SetPlayerDrilling(false);
        }

        void Update()
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, nextRotation, rotationSpeed * Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (rigidbody.velocity.magnitude <= maxSpeed) {
                rigidbody.AddForce(movementSpeedConstant * playerInputHandler.MovementVector3);
            }

            if (!Mathf.Approximately(playerInputHandler.MovementVector3.magnitude, 0)) {
                nextRotation = Quaternion.LookRotation(playerInputHandler.MovementVector3, Vector3.up);
            }
        }
    }
}
