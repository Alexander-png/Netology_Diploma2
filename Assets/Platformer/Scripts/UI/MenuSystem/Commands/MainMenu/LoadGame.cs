using Platformer.GameCore;
using Platformer.UI.MenuSystem.Commands.Base;

namespace Platformer.UI.MenuSystem.Commands.MainMenu
{
	public class LoadGame : MenuCommand
	{
        public override void Execute(object data)
        {
            GameObserver.NewGameFlag = false;
            GameObserver.SwitchScene(SceneTypes.Level1);
        }
	}
}