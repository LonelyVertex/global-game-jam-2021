using UnityEngine;
using Zenject;

namespace DI.Installers
{
    public class ProjectContextMonoInstaller : MonoInstaller<ProjectContextMonoInstaller>
    {
        /**
         * Just a sample how we can define the same interface and decide how the implementation will be in different
         * environment.
         */
        public override void InstallBindings()
        {
#if UNITY_EDITOR
            Container.Bind<ILogger>().FromInstance(Debug.unityLogger).AsSingle();
#else
            Container.Bind<ILogger>().To<Logger.ProductionLogger>().AsSingle();
#endif
        }
    }
}
