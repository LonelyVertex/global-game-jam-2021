using System;
using Cinemachine;
using Player.Energy;
using UnityEngine;
using Zenject;

namespace Traps
{
    public class MineTrap : AbstractTrap
    {
        // TODO: MOVE TO GAME SETTINGS / FLOOR DIFICULTY SETTINGS ???
        const float percentageEnergyDrain = 0.1f;

        [Inject]
        EnergyHandler energyHandler;

        [SerializeField] 
        CinemachineImpulseSource impulseSource;

        public event Action<MineTrap> OnTrapTrigger;

        protected override void TriggerTrap()
        {
            impulseSource.GenerateImpulse();
            energyHandler.DrainPercentage(percentageEnergyDrain);
            OnTrapTrigger?.Invoke(this);
        }

        void Initialize(Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
        }

        public class Factory : MonoMemoryPool<Transform, MineTrap>
        {
            protected override void Reinitialize(Transform parent, MineTrap item)
            {
                item.Initialize(parent);
            }
        }
    }
}
