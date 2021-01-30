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

        [Inject(Id = Identifiers.PlayerTransform)]
        Transform playerTransform;

        void Start()
        {
            cinemachineVirtualCamera.Follow = playerTransform;
        }
    }
}
