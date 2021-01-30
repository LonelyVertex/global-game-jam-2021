using Player.Controls;
using Player.Energy;
using UnityEngine;
using Zenject;

namespace DI.Installers
{
    public class GameplayInstaller : MonoInstaller<GameplayInstaller>
    {
        [SerializeField]
        int playerInitialEnergy = 1000;

        [SerializeField]
        int initialTickRate = 10;

        public override void InstallBindings()
        {
            Container.Bind<PlayerInputHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnergyHandler>().FromInstance(new EnergyHandler(maxEnergy: playerInitialEnergy, energyTickRate: initialTickRate));
        }
    }
}
