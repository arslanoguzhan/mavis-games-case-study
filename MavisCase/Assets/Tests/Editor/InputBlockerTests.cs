using MavisCase.Common.InputSystem;
using NUnit.Framework;

namespace Tests
{
    public class InputBlockerTests
    {
        private InputBlocker _blocker;

        [SetUp]
        public void Setup()
        {
            _blocker = new InputBlocker();
        }

        [Test]
        public void Input_IsEnabled_ByDefault()
        {
            Assert.IsTrue(_blocker.IsInputEnabled);
            Assert.IsFalse(_blocker.IsInputDisabled);
        }

        [Test]
        public void Block_DisablesInput()
        {
            _blocker.Block();

            Assert.IsFalse(_blocker.IsInputEnabled);
            Assert.IsTrue(_blocker.IsInputDisabled);
        }

        [Test]
        public void MultipleBlocks_StackProperly()
        {
            _blocker.Block();
            _blocker.Block();

            Assert.IsFalse(_blocker.IsInputEnabled);
            Assert.IsTrue(_blocker.IsInputDisabled);
        }

        [Test]
        public void Unblock_DecreasesStack()
        {
            _blocker.Block(); // 1
            _blocker.Block(); // 2
            _blocker.Unblock(); // 1

            Assert.IsTrue(_blocker.IsInputDisabled);
            Assert.IsFalse(_blocker.IsInputEnabled);
        }

        [Test]
        public void Unblock_ToZero_EnablesInput()
        {
            _blocker.Block();
            _blocker.Unblock();

            Assert.IsTrue(_blocker.IsInputEnabled);
            Assert.IsFalse(_blocker.IsInputDisabled);
        }

        [Test]
        public void Unblock_BelowZero_DoesNotCrash()
        {
            _blocker.Unblock(); // from 0 to -1 (clamped to 0)

            Assert.IsTrue(_blocker.IsInputEnabled);
            Assert.IsFalse(_blocker.IsInputDisabled);
        }

        [Test]
        public void ManyUnblocks_DoesNotGoNegative()
        {
            _blocker.Block(); // 1

            _blocker.Unblock(); // 0
            _blocker.Unblock(); // stays 0
            _blocker.Unblock(); // stays 0

            Assert.IsTrue(_blocker.IsInputEnabled);
            Assert.IsFalse(_blocker.IsInputDisabled);
        }
    }

}