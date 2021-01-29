using Player.Controls;
using Zenject;

namespace DI.Installers
{
    public class GameplayInstaller : MonoInstaller<GameplayInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerInputHandler>().AsSingle();
        }
    }
}
