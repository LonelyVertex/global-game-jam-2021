using System;
using Player.Energy;
using UnityEngine;
using Zenject;

namespace Traps
{
    public class MineTrap : AbstractTrap
    {

        const float percentageEnergyDrain = 0.1f;

        [Inject]
        EnergyHandler energyHandler;

        public event Action<MineTrap> OnTrapTrigger;

        protected override void TriggerTrap()
        {
            energyHandler.DrainPercentage(percentageEnergyDrain);
            OnTrapTrigger?.Invoke(this);
        }

        void Initialize(Vector3 position)
        {
            transform.position = position;
        }

        public class Factory : MonoMemoryPool<Vector3, MineTrap>
        {
            protected override void Reinitialize(Vector3 p1, MineTrap item)
            {
                item.Initialize(p1);
            }
        }
    }
}
