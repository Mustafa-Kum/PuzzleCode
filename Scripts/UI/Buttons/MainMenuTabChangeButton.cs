using _Game.Scripts.Managers.Core;

namespace _Game.Scripts.UI.Buttons
{
    public class MainMenuTabChangeButton : ButtonBase
    {
        protected override void OnClicked()
        {
            EventManager.UIEvents.OnMenuTabsChanged?.Invoke();
        }
    }
}