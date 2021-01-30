using UnityEngine;
using Utils;
using Zenject;

namespace Player.Energy
{
    public class SafeZoneEnergyTickTrigger : MonoBehaviour
    {
        [Inject]
        EnergyHandler energyHandler;

        void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider.CompareTag(Tags.SAFE_ZONE)) {
                energyHandler.EnableTicking(enableTicking: false);
            }
        }

        void OnTriggerExit(Collider otherCollider)
        {
            if (otherCollider.CompareTag(Tags.SAFE_ZONE)) {
                energyHandler.EnableTicking(enableTicking: true);
            }
        }
    }
}
