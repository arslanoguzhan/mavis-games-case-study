using System;
using MavisCase.Common.Communication;
using MavisCase.Common.Signals;
using Zenject;

namespace MavisCase.HomeScreen
{
    public class MainMenuController : IInitializable, IDisposable
    { 
        private MainMenu _menu;
        private GlobalSignalBus _globalSignalBus;
        
        public MainMenuController(MainMenu menu, GlobalSignalBus globalSignalBus)
        {
            _menu = menu;
            _globalSignalBus = globalSignalBus;
        }
        
        private void OnReturnToHomeSceneSignal(ReturnToHomeSignal  signal)
        {
            _menu.OnReturnToHome();
        }

        public void Initialize()
        {
            _globalSignalBus.SignalBus.Subscribe<ReturnToHomeSignal>(OnReturnToHomeSceneSignal);
        }

        public void Dispose()
        {
            _globalSignalBus.SignalBus.Unsubscribe<ReturnToHomeSignal>(OnReturnToHomeSceneSignal);
        }
    }
}