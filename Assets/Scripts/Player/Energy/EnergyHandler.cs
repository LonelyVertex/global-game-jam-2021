using Player.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Player.Energy
{
    public class EnergyHandler: ITickable
    {
        const float ENERGY_TICKS_PER_SECOND = 1.0f;
        float previousTickTime = 0.0f;
        ILogger logger;

        public bool IsEnergyTickingEnabled { get; private set; }
        public int CurrentEnergy { get; private set; }
        public int EnergyTickRate { get; set; }
        public int MaxEnergy { get; set; }

        public EnergyHandler(PlayerInitialConfiguration configuration, ILogger logger)
        {
            this.logger = logger;
            EnergyTickRate = configuration.InitialTickRate;
            MaxEnergy = configuration.PlayerInitialEnergy;
            RefillEnergy();
        }

        public void TickEnergy()
        {
            CurrentEnergy -= EnergyTickRate;
            logger.Log($"CurrentEnergy: {CurrentEnergy}");
        }

        public void RefillEnergy()
        {
            CurrentEnergy = MaxEnergy;
        }

        public void EnableTicking(bool enableTicking = true)
        {
            IsEnergyTickingEnabled = enableTicking;
        }

        public void Tick()
        {
            var shouldTickEnergy = previousTickTime + 1 / ENERGY_TICKS_PER_SECOND < Time.time;
            if (IsEnergyTickingEnabled && shouldTickEnergy) {
                TickEnergy();
                previousTickTime = Time.time;
            }
        }
    }
}
