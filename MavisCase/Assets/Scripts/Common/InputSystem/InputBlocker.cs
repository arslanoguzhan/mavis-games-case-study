using System;

namespace MavisCase.Common.InputSystem
{
    public class InputBlocker
    {
        private int _stack = 0;

        public bool IsInputEnabled => _stack == 0;

        public bool IsInputDisabled => _stack > 0;

        public void Unblock()
        {
            _stack = Math.Max(0, _stack-1);
        }

        public void Block()
        {
            _stack++;
        }
    }
}
