using Cysharp.Threading.Tasks;
using MavisCase.Common.Scenes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MavisCase.HomeScreen
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _ticTacToeButton;
        [SerializeField] private Button _memoryGameButton;
        [Inject] private SceneLoader _sceneLoader;
        
        private void Awake()
        {
            _ticTacToeButton.onClick.AddListener(LoadTicTacToe);
            _memoryGameButton.onClick.AddListener(LoadMemoryGame);
        }

        public void OnReturnToHome()
        {
            gameObject.SetActive(true);
        }

        private void LoadMemoryGame()
        {
            _sceneLoader.LoadSceneAdditive(SceneMap.MemoryGame).Forget();
            gameObject.SetActive(false);
        }
        
        private void LoadTicTacToe()
        {
            _sceneLoader.LoadSceneAdditive(SceneMap.TicTacToe).Forget();
            gameObject.SetActive(false);
        }
    }
}