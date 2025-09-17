using System;
using DG.Tweening;
using MavisCase.Common.Communication;
using MavisCase.Common.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MavisCase.Common.Panels
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private GameObject _loadingPanelText;
        [SerializeField] private GraphicRaycaster _raycaster;
        
        private GlobalSignalBus _globalSignalBus;

        private Tween _fadeTween;
        
        [Inject]
        public void Inject(GlobalSignalBus signalBus)
        {
            _globalSignalBus = signalBus;
        }

        private void OnEnable()
        {
            _globalSignalBus.SignalBus.Subscribe<CloseLoadingPanelSignal>(OnCloseLoadingPanelSignal);
            _globalSignalBus.SignalBus.Subscribe<ShowLoadingPanelSignal>(OnShowLoadingPanelSignal);
        }
        
        private void OnDestroy()
        {
            _globalSignalBus.SignalBus.Unsubscribe<CloseLoadingPanelSignal>(OnCloseLoadingPanelSignal);
            _globalSignalBus.SignalBus.Unsubscribe<ShowLoadingPanelSignal>(OnShowLoadingPanelSignal);
        }

        private void OnCloseLoadingPanelSignal(CloseLoadingPanelSignal signal)
        {
            _fadeTween?.Kill();
            _raycaster.enabled = false;
            _fadeTween = _backgroundImage.DOFade(0f, 0.3f);
            _loadingPanelText.gameObject.SetActive(false);
        }

        private void OnShowLoadingPanelSignal(ShowLoadingPanelSignal signal)
        {
            _fadeTween?.Kill();
            _raycaster.enabled = true;
            _fadeTween = _backgroundImage.DOFade(1f, 0f);
            _loadingPanelText.gameObject.SetActive(true);
        }
    }
}