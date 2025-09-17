using Cysharp.Threading.Tasks;
using MavisCase.Common.Communication;
using MavisCase.Common.Persistence;
using MavisCase.Common.Signals;
using Zenject;

namespace MavisCase.Games.TicTacToeGame
{
    public class EntryPoint : IInitializable
    {
        private GlobalSignalBus _globalSignalBus;
        private ProgressStorage<Progress> _progressStorage;
        private UserStorage _userStorage;
        private GameManager _gameManager;
        
        public EntryPoint(GlobalSignalBus globalSignalBus, ProgressStorage<Progress> progressStorage, UserStorage userStorage, GameManager gameManager)
        {
            _globalSignalBus = globalSignalBus;
            _progressStorage = progressStorage;
            _userStorage = userStorage;
            _gameManager = gameManager;
        }

        public void Initialize()
        {
            StartAsync().Forget();
        }

        private async UniTaskVoid StartAsync()
        {
            await _userStorage.LoadUserAsync();
            await _progressStorage.LoadProgressAsync(Progress.CreateInitialProgress);
            _gameManager.OnInitialize();
            await UniTask.Delay(400);
            _globalSignalBus.SignalBus.Fire(new CloseLoadingPanelSignal());
        }
    }
}