using Generator;
using Player.Controls;
using Player.Energy;
using Player.State;
using Resources;
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

        [SerializeField]
        GameObject generatorPrefab;

        [SerializeField]
        GameObject minePrefab;

        [SerializeField]
        GameObject resourceBoxPrefab;

        public override void InstallBindings()
        {
            Container.Bind<PlayerInputHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnergyHandler>().AsSingle();
            Container.Bind<PlayerStateComponent>().FromComponentInHierarchy().AsSingle(); // TODO: @Trilka je tohle dobře??? potřebuju komponentu co se instancuje pomocí DI containeru z Prefabu... :/
            Container.Bind<CharacterController>().WithId(Identifiers.PlayerCharacterController).FromComponentInNewPrefab(playerPrefab).AsSingle();
            Container.Bind<MineTrapGenerator>().AsSingle();
            Container.BindMemoryPool<MineTrap, MineTrap.Factory>().WithInitialSize(4).FromComponentInNewPrefab(minePrefab);
            Container.Bind<LevelGenerator>().FromComponentInNewPrefab(generatorPrefab).AsSingle();
            Container.Bind<GameManager>().AsSingle();
            Container.Bind<ResourceBoxGenerator>().AsSingle();
            Container.BindMemoryPool<ResourceBox, ResourceBox.Factory>().WithInitialSize(4)
                .FromComponentInNewPrefab(resourceBoxPrefab);
        }
    }
}
