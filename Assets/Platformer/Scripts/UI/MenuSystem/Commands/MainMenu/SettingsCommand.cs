using Platformer.UI.MenuSystem.Commands.Base;

namespace Platformer.UI.MenuSystem.Commands.MainMenu
{
	public class SettingsCommand : MenuCommand
	{
        public override void Execute()
        {
            EditorExtentions.GameLogger.AddMessage("Todo: settings");
        }
	}
}