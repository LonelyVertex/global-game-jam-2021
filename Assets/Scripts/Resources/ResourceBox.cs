using System;
using Player.State;
using UnityEngine;
using Utils;
using Zenject;

namespace Resources
{
    public class ResourceBox : MonoBehaviour
    {
        [Inject]
        GameManager gameManager;

        [Inject] 
        PlayerStateComponent playerState;

        bool isPlayerInRange = false;

        public event Action<ResourceBox> OnBoxDrilled;

        float drilledTime = 0.0f;
        // TODO: MOVE TO GAME SETTINGS
        const float NEEDED_DRILL_TIME_SECONDS = 1.2f;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.PLAYER)) {
                isPlayerInRange = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.PLAYER)) {
                isPlayerInRange = false;
            }
        }

        void Initialize(Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            drilledTime = 0.0f;
            isPlayerInRange = false;
        }

        void Update()
        {
            if (playerState.IsDrilling && isPlayerInRange) {
                drilledTime += Time.deltaTime;
            }

            if (drilledTime > NEEDED_DRILL_TIME_SECONDS) {
                OnBoxDrilled?.Invoke(this);
            }
        }

        public class Factory: MonoMemoryPool<Transform, ResourceBox>
        {
            protected override void Reinitialize(Transform parent, ResourceBox item)
            {
                item.Initialize(parent);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}
