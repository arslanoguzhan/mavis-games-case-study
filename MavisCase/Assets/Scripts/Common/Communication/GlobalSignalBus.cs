using Zenject;

namespace MavisCase.Common.Communication
{
    public class GlobalSignalBus
    {
        public SignalBus SignalBus { get; private set; }
        
        public GlobalSignalBus(SignalBus signalBus)
        {
            SignalBus = signalBus;
        }
    }
}