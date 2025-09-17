using System;
using System.Collections.Generic;
using System.Linq;
using MavisCase.Common.Assets;
using MavisCase.Common.Communication;
using MavisCase.Common.Signals;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace MavisCase.Common.Popups
{
    public class PopupService : IInitializable, IDisposable
    {
        private Dictionary<Type, IPopup> _popups = new();
        private HashSet<Type> _activePopups = new();
        
        private AssetManager _assetManager;
        private SignalBus _signalBus;
        
        public PopupService(AssetManager assetManager, SignalBus signalBus)
        {
            _assetManager = assetManager;
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            SubscribeSignals();
        }

        public void Dispose()
        {
            UnsubscribeSignals();
        }
        
        private void SubscribeSignals()
        {
            _signalBus.Subscribe<CloseAllPopupsSignal>(OnCloseAllPopups);
        }

        private void UnsubscribeSignals()
        {
            _signalBus.Unsubscribe<CloseAllPopupsSignal>(OnCloseAllPopups);
        }
        
        public T ShowPopup<T>(string directory, PopupArguments arguments) where T : Object, IPopup
        {
            var popupType = typeof(T);
            if (!_popups.ContainsKey(popupType))
            {
                var popupAsset = _assetManager.GetAsset<T>(directory);
                var popupObj = GameObject.Instantiate(popupAsset);
                _popups[popupType] =  popupObj;
            }
            
            var popup = _popups[popupType];
            if (_activePopups.Contains(popupType))
            {
                return (T)popup;
            }
                
            popup.Show(arguments);
            _activePopups.Add(popupType);
            return (T)popup;
        }

        public void HidePopup(Type popupType)
        {
            if (_popups.ContainsKey(popupType))
            {
                var popup = _popups[popupType];
                popup.Hide();
                _activePopups.Remove(popupType);
            }
        }

        private void OnCloseAllPopups(CloseAllPopupsSignal obj)
        {
            while (_activePopups.Count > 0)
            {
                HidePopup(_activePopups.First());
            }
        }
    }

}