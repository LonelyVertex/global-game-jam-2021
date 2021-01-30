using UnityEngine;

namespace Player.State
{
    public class PlayerStateComponent : MonoBehaviour
    {
        public Vector3 GetPlayerPosition()
        {
            return transform.position;
        }
    }
}
