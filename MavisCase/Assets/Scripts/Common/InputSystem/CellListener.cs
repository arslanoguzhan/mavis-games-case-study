using MavisCase.Common.Communication;
using MavisCase.Common.InputKinds;
using MavisCase.Common.Pooling;
using MavisCase.Common.Signals;
using UnityEngine;
using Zenject;

namespace MavisCase.Common.InputSystem
{
    public class CellListener : MonoBehaviour, IPoolItem,
        IListener<TouchTap>,
        IListener<MouseLeftClick>
    {
        private int _cellIndex;
        
        private SignalBus _signalBus;

        [Inject] 
        public void Inject(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void SetCellIndex(int cellIndex)
        {
            _cellIndex = cellIndex;
        }
        
        void IListener<TouchTap>.OnInteraction()
        {
            _signalBus.Fire(new TappedCellSignal(){ CellIndex = _cellIndex});
        }

        void IListener<MouseLeftClick>.OnInteraction()
        {
            _signalBus.Fire(new TappedCellSignal(){ CellIndex = _cellIndex});
        }

        public void Recycle()
        {
        }
    }
}