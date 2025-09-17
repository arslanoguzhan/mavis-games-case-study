using MavisCase.Common;
using MavisCase.Common.Assets;
using MavisCase.Common.Communication;
using MavisCase.Common.GridSystem;
using MavisCase.Common.InputHandlers;
using MavisCase.Common.InputSystem;
using MavisCase.Common.Persistence;
using MavisCase.Common.Pooling;
using MavisCase.Common.Popups;
using UnityEngine;
using Zenject;

namespace MavisCase.Games.TicTacToeGame
{
    [AddComponentMenu("TicTacToeGame.Installer")]
    public class Installer : MonoInstaller
    {
        [SerializeField]
        private ItemLookup ItemLookup;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EntryPoint>().AsSingle().NonLazy();
            
            Container.Bind<ProgressStorage<Progress>>().AsSingle();
            Container.Bind<UserStorage>().AsSingle();
            
            Container.Bind<InputBlocker>().To<InputBlocker>().AsSingle().NonLazy();
            Container.Bind<AssetManager>().AsSingle().NonLazy();
            Container.Bind<PoolService>().AsSingle().NonLazy();
            
            Container.BindInterfacesTo<GridManager>().AsSingle().NonLazy();
            Container.Bind<GraphicManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PopupService>().AsSingle().NonLazy();
            Container.BindInstance(new SignalBus()).AsSingle();
            
            Container.BindInterfacesAndSelfTo<TouchHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MouseLeftButtonHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MouseRightButtonHandler>().AsSingle().NonLazy();
            
            Container.Bind<IGamePrefix>().To<GamePrefix>().AsSingle().NonLazy();
            Container.Bind<IInputConfig>().To<InputConfig>().AsSingle().NonLazy();
            Container.Bind<IGridConfig>().To<GridConfig>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
            Container.BindInstance(ItemLookup).AsSingle();
        }
    }
}