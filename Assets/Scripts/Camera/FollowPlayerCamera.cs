using Cinemachine;
using UnityEngine;
using Utils.DI;
using Zenject;

namespace Camera
{
    public class FollowPlayerCamera : MonoBehaviour
    {
        [SerializeField]
        CinemachineVirtualCamera cinemachineVirtualCamera;

        [Inject(Id = Identifiers.PlayerCharacterController)]
        CharacterController playerCharacterController;

        void Start()
        {
            cinemachineVirtualCamera.Follow = playerCharacterController.transform;
        }
    }
}
