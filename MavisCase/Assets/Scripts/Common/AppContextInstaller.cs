using MavisCase.Common.Communication;
using MavisCase.Common.Persistence;
using MavisCase.Common.Scenes;
using MavisCase.Common.Serialization;
using UnityEngine;
using Zenject;

namespace MavisCase.Common
{
    public class AppContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(Camera.main).AsSingle().NonLazy();
            Container.Bind<FileManager>().To<FileManager>().AsSingle().NonLazy();
            Container.Bind<JsonSerializer>().To<JsonSerializer>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();
            Container.Bind<GlobalSignalBus>().AsSingle().WithArguments(new object[] { new SignalBus() });
        }
    }
}