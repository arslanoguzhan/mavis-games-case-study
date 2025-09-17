using DG.Tweening;
using MavisCase.Common.Pooling;
using TMPro;
using UnityEngine;

namespace MavisCase.Games.MemoryGame
{
    [AddComponentMenu("MemoryGame.CardView")]
    public class CardView : MonoBehaviour, IPoolItem
    {
        [SerializeField] private TMP_Text _letter;
        [SerializeField] private SpriteRenderer _overlay;

        private Tween _fadeTween;
        private Tween _delayedFadeTween;
        
        public void SetLetter(byte letter)
        {
            _letter.SetCharArray(new []{(char)letter});
        }

        public void OnOpen()
        {
            _delayedFadeTween?.Kill();
            _fadeTween?.Kill();
            
            _fadeTween = _overlay.DOFade(0f, 0.25f);
        }

        public void OnCloseDelayed()
        {
            _delayedFadeTween = DOVirtual.DelayedCall(0.25f, () =>
            {
                _fadeTween = _overlay.DOFade(1f, 0.25f);
            });
        }
        
        public void OnDisappearDelayed()
        {
            transform.DOScale(Vector2.zero, 0.25f).SetDelay(0.45f);
        }

        public void Recycle()
        {
            _fadeTween?.Kill();
            _delayedFadeTween?.Kill();
            _overlay.DOFade(1, 0f);
            transform.localScale = Vector2.one * 0.8f;
            gameObject.SetActive(false);
        }
    }
}