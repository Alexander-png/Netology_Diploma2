using Platformer.GameCore;
using Platformer.UI.MenuSystem.Commands.Base;

namespace Platformer.UI.MenuSystem.Commands.MainMenu
{
	public class LoadGame : MenuCommand
	{
        public override void Execute()
        {
            GameObserver.NewGameFlag = false;
            GameObserver.SwitchScene(SceneTypes.Game);
        }
	}
}