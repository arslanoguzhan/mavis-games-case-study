using System;
using Cysharp.Threading.Tasks;
using MavisCase.Common.Communication;
using MavisCase.Common.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace MavisCase.Common.Scenes
{
    public class SceneLoader : IInitializable, IDisposable
    {
        private Scene _currentScene;
        
        private GlobalSignalBus _globalSignalBus;
        
        public SceneLoader(GlobalSignalBus globalSignalBus)
        {
            _globalSignalBus = globalSignalBus;
        }

        public void Initialize()
        {
            _globalSignalBus.SignalBus.Subscribe<ReturnToHomeSignal>(ReturnHomeScene);
        }

        public void Dispose()
        {
            _globalSignalBus.SignalBus.Unsubscribe<ReturnToHomeSignal>(ReturnHomeScene);
        }

        public async UniTask LoadSceneAdditive(SceneMap scene)
        {
            _globalSignalBus.SignalBus.Fire(new ShowLoadingPanelSignal());
            await SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
            var newScene = SceneManager.GetSceneByBuildIndex((int)scene);
            SceneManager.SetActiveScene(newScene);
            _currentScene = newScene;
        }

        public void ReturnHomeScene(ReturnToHomeSignal returnToHomeSignal)
        {
            SceneManager.UnloadSceneAsync(_currentScene);
        }
    }
}