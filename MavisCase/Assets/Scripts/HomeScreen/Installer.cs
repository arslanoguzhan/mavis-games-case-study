using MavisCase.Common.Assets;
using MavisCase.Common.InputHandlers;
using MavisCase.Common.InputSystem;
using MavisCase.Common.Persistence;
using MavisCase.Common.Serialization;
using UnityEngine;
using Zenject;

namespace MavisCase.HomeScreen
{
    [AddComponentMenu("HomeScreen.Installer")]
    public class Installer : MonoInstaller
    {
        [SerializeField] private MainMenu _menu;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainMenuController>().AsSingle().NonLazy();
            
            Container.BindInstance(_menu).AsSingle().NonLazy();
        }
    }    
}
