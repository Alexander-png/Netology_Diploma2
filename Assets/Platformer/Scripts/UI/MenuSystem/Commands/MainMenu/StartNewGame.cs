using Platformer.GameCore;
using Platformer.UI.MenuSystem.Commands.Base;

namespace Platformer.UI.MenuSystem.Commands.MainMenu
{
	public class StartNewGame : MenuCommand
	{
        public override void Execute()
        {
            GameObserver.NewGameFlag = true;
            GameObserver.SwitchScene(SceneTypes.Level1);
        }
	}
}