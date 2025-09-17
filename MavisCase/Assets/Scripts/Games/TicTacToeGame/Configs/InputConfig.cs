using MavisCase.Common.InputSystem;

namespace MavisCase.Games.TicTacToeGame
{
    public class InputConfig : IInputConfig
    {
        public float SwipeThreshold { get; private set; } = 500f;
        public float HoldDuration { get; private set; } = 0.3f;
    }
}
