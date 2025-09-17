using MavisCase.Common.Communication;
using MavisCase.Common.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace MavisCase.Games.TicTacToeGame
{
    public class TicTacToeGamePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelText;
        
        private SignalBus _signalBus;
        
        [Inject]
        private void Inject(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<LevelUpdatedSignal>(OnLevelUpdated);
        }
        
        private void OnDisable()
        {
            _signalBus.Unsubscribe<LevelUpdatedSignal>(OnLevelUpdated);
        }

        private void OnLevelUpdated(LevelUpdatedSignal signal)
        {
            _levelText.SetText($"Level {signal.Level}");
        }
    }
}