using System;
using System.Collections.Generic;
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

        List<Action<MineTrap>> onTrapTrigger = new List<Action<MineTrap>>();
        public event Action<MineTrap> OnTrapTrigger {
            add => onTrapTrigger.Add(value);
            remove => onTrapTrigger.Remove(value);
        }

        protected override void TriggerTrap()
        {
            energyHandler.DrainPercentage(percentageEnergyDrain);
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
