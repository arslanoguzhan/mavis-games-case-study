using DG.Tweening;
using MavisCase.Common.Pooling;
using TMPro;
using UnityEngine;

namespace MavisCase.Games.TicTacToeGame
{
    [AddComponentMenu("TicTacToeGame.MarkView")]
    public class MarkView : MonoBehaviour, IPoolItem
    {
        [SerializeField] private TextMeshPro _text;

        private Tween _scaleTween;
        
        public void UpdateAsPlayer()
        {
            transform.localScale = Vector3.zero;
            _scaleTween = transform.DOScale(1f, 0.15f);
            _text.SetText("X");
        }

        public void UpdateAsComputer()
        {
            transform.localScale = Vector3.zero;
            _text.SetText("O");
            _scaleTween = transform.DOScale(1f, 0.15f).SetDelay(0.2f);
        }

        public void Recycle()
        {
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
            _scaleTween.Kill();
        }
    }
}