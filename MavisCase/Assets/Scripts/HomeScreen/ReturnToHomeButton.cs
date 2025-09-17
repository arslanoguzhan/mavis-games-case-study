using MavisCase.Common.Communication;
using MavisCase.Common.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MavisCase.HomeScreen
{
    public class ReturnToHomeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private GlobalSignalBus _globalSignalBus;

        [Inject]
        public void Inject(GlobalSignalBus globalSignalBus)
        {
            _globalSignalBus = globalSignalBus;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(ReturnToHome);
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void ReturnToHome()
        {
            _globalSignalBus.SignalBus.Fire(new ReturnToHomeSignal());
        }
    }
}