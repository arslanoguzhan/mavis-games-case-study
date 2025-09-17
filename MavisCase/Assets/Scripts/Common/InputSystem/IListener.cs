namespace MavisCase.Common.InputSystem
{
    interface IListener {}

    interface IListener<TInputKind> : IListener where TInputKind : IInputKind
    {
        void OnInteraction();
    }
}
