using Player.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace DI.Installers
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GameplayScriptableObjectInstaller")]
    public class GameplayScriptableObjectInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        PlayerInitialConfiguration playerInitialConfiguration;

        public override void InstallBindings()
        {
            Container.Bind<PlayerInitialConfiguration>().FromScriptableObject(playerInitialConfiguration).AsSingle();
        }
    }
}
