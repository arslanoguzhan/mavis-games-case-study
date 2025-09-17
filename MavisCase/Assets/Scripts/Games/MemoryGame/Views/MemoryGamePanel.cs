using System;
using MavisCase.Common.Communication;
using MavisCase.Common.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace MavisCase.Games.MemoryGame
{
    public class MemoryGamePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _movesText;
        [SerializeField] private TMP_Text _levelText;
        
        [Inject] private SignalBus _signalBus;
        
        private void OnEnable()
        {
            _signalBus.Subscribe<MoveChangedSignal>(OnMoveChangedSignal);
            _signalBus.Subscribe<LevelUpdatedSignal>(OnLevelUpdatedSignal);
            _movesText.SetText($"Moves: {Constants.MoveCount}");
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<MoveChangedSignal>(OnMoveChangedSignal);
            _signalBus.Unsubscribe<LevelUpdatedSignal>(OnLevelUpdatedSignal);
        }

        private void OnLevelUpdatedSignal(LevelUpdatedSignal obj)
        {
            _levelText.SetText($"Level {obj.Level}");
        }

        private void OnMoveChangedSignal(MoveChangedSignal signal)
        {
            _movesText.SetText($"Moves: {signal.Count}");
        }
    }
}