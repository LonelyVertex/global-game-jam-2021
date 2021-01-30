using UnityEngine;
using Zenject;

namespace Player.Energy
{
    public class EnergyHandler: ITickable
    {
        const float ENERGY_TICKS_PER_SECOND = 1.0f;
        float previousTickTime = 0.0f;

        public bool IsEnergyTickingEnabled { get; private set; }
        public int CurrentEnergy { get; private set; }
        public int EnergyTickRate { get; set; }
        public int MaxEnergy { get; set; }

        public EnergyHandler(int maxEnergy = 1000, int energyTickRate = 10)
        {
            EnergyTickRate = energyTickRate;
            MaxEnergy = maxEnergy;
            RefillEnergy();
        }

        public void TickEnergy()
        {
            CurrentEnergy -= EnergyTickRate;
            Debug.Log($"CurrentEnergy: {CurrentEnergy}");
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
