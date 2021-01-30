using UnityEngine;

namespace Player.State
{
    public class PlayerStateComponent : MonoBehaviour
    {
        public bool IsDrilling { get; set; }
        
        public Vector3 GetPlayerPosition()
        {
            return transform.position;
        }
    }
}
