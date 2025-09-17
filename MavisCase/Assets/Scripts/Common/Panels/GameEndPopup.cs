using MavisCase.Common.Communication;
using MavisCase.Common.Popups;
using MavisCase.Common.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MavisCase.Common.Panels
{
    public class GameEndPopupArguments : PopupArguments
    {
        public string Text;
        public string ButtonText;
    }
    
    public class GameEndPopup : MonoBehaviour, IPopup
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private TMP_Text _gameEndText;
        [SerializeField] private TMP_Text _gameEndBtnText;
        
        private SignalBus _signalBus;

        [Inject] 
        public void Inject(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Show(PopupArguments arguments)
        {
            var args = (GameEndPopupArguments)arguments;

            _gameEndText.SetText(args.Text);
            _gameEndBtnText.SetText(args.ButtonText);
            
            _restartButton.onClick.AddListener(OnGameEnd);
            
            gameObject.SetActive(true);
        }

        private void OnGameEnd()
        {
            _signalBus.Fire(new CloseAllPopupsSignal());
            _signalBus.Fire(new PostLevelSignal());
        }

        public void Hide()
        {
            _restartButton.onClick.RemoveAllListeners();
            gameObject.SetActive(false);
        }
    }
}