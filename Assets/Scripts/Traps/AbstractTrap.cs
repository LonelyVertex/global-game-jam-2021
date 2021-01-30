using UnityEngine;
using Utils;

namespace Traps
{
    public abstract class AbstractTrap : MonoBehaviour
    {
        protected abstract void TriggerTrap();

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.PLAYER)) {
                TriggerTrap();
            }
        }
    }
}
