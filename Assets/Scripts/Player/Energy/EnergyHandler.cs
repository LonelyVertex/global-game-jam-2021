using System;
using Player.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Player.Energy
{
    public class EnergyHandler: ITickable
    {
        int currentEnergy;

        // TODO: MOVE TO GAME SETTINGS
        const float ENERGY_TICKS_PER_SECOND = 1.0f;
        float previousTickTime = 0.0f;
        ILogger logger;

        public event Action OnEnergyDepleeted;

        public bool IsEnergyTickingEnabled { get; private set; }
        public int CurrentEnergy {
            get => currentEnergy;
            private set {
                currentEnergy = value;
                if (value <= 0) {
                    currentEnergy = 0;
                    logger.Log($"Energy drained to: {currentEnergy}");

                    OnEnergyDepleeted?.Invoke();
                    return;
                }

                logger.Log($"CurrentEnergy: {currentEnergy}");
            }
        }
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

        public void DrainPercentage(float percentageEnergyDrain)
        {
            CurrentEnergy -= Mathf.CeilToInt(MaxEnergy * percentageEnergyDrain);
        }
    }
}
