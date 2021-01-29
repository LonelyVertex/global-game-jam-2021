using UnityEngine;
using Zenject;

namespace Player.Controls
{
    public class PlayerControlsComponent : MonoBehaviour
    {
        [Inject]
        PlayerInputHandler playerInputHandler;

        [SerializeField]
        Rigidbody rigidbody;

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
        }

        void OnDisable()
        {
            playerInputHandler.DisablePlayerInput();
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
