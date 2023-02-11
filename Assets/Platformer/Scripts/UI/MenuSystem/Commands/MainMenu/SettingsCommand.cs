using Platformer.UI.MenuSystem.Commands.Base;

namespace Platformer.UI.MenuSystem.Commands.MainMenu
{
	public class SettingsCommand : MenuCommand
	{
        public override void Execute(object data)
        {
            EditorExtentions.GameLogger.AddMessage("Todo: settings");
            // Something like this:
            //SettingMenu.Enter(AssociatedMenu);
        }
    }
}