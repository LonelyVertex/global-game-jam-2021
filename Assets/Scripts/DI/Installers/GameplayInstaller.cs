using Generator;
using Player.Controls;
using Player.Energy;
using Player.State;
using Resources;
using Traps;
using UnityEngine;
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
        GameObject mineExplosionPrefab;

        [SerializeField]
        GameObject resourceBoxPrefab;

        [SerializeField] 
        GameObject globalAudioPrefab;

        public override void InstallBindings()
        {
            Container.Bind<PlayerInputHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnergyHandler>().AsSingle(); 
            Container.Bind(typeof(CharacterController), typeof(PlayerStateComponent))
                .FromComponentInNewPrefab(playerPrefab).AsSingle();
            Container.Bind<MineTrapGenerator>().AsSingle();
            Container.BindMemoryPool<MineTrap, MineTrap.Factory>().WithInitialSize(4).FromComponentInNewPrefab(minePrefab);
            Container.Bind<LevelGenerator>().FromComponentInNewPrefab(generatorPrefab).AsSingle();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
            Container.Bind<ResourceBoxGenerator>().AsSingle();
            Container.BindMemoryPool<ResourceBox, ResourceBox.Factory>().WithInitialSize(4)
                .FromComponentInNewPrefab(resourceBoxPrefab);
            Container.BindMemoryPool<MineTrapEffect, MineTrapEffect.EffectFactory>().WithInitialSize(4)
                .FromComponentInNewPrefab(mineExplosionPrefab);
            Container.Bind<GlobalAudio>().FromComponentInNewPrefab(globalAudioPrefab).AsSingle();
        }
    }
}
