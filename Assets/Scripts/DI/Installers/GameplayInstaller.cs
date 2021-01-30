using Generator;
using Player.Controls;
using Player.Energy;
using Traps;
using UnityEngine;
using Utils.DI;
using Zenject;

namespace DI.Installers
{
    public class GameplayInstaller : MonoInstaller<GameplayInstaller>
    {
        [SerializeField]
        GameObject playerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<PlayerInputHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnergyHandler>().AsSingle();
            Container.Bind<Transform>().WithId(Identifiers.PlayerTransform).FromComponentInNewPrefab(playerPrefab).AsSingle();
            Container.Bind<MineTrapGenerator>().AsSingle();
            Container.BindMemoryPool<MineTrap, MineTrap.Factory>().AsSingle();
        }
    }
}
