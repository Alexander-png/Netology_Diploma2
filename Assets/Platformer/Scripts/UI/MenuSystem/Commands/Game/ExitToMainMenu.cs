using Platformer.GameCore;
using Platformer.UI.MenuSystem.Commands.Base;

namespace Platformer.UI.MenuSystem.Commands.Game
{
	public class ExitToMainMenu : MenuCommand
	{
        public override void Execute()
        {
            GameObserver.SwitchScene(SceneTypes.MainMenu);
        }
	}
}