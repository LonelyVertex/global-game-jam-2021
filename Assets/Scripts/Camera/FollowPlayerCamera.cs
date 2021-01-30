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

        CinemachineFramingTransposer framingTransposer;

        [Inject]
        CharacterController playerCharacterController;

        void Start()
        {
            cinemachineVirtualCamera.Follow = playerCharacterController.transform;
            framingTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            framingTransposer.m_SoftZoneHeight = 0.5195311f;
            framingTransposer.m_SoftZoneWidth = 0.5472223f;
        }
    }
}
