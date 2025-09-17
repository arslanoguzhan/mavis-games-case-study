namespace MavisCase.Common.Popups
{
    public interface IPopup
    {
        void Show(PopupArguments arguments);
        void Hide();
    }
}