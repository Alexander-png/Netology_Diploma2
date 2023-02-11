using Platformer.EditorExtentions;
using Platformer.GameCore;
using Platformer.UI.MenuSystem.Commands.Base;

namespace Platformer.UI.MenuSystem.Commands.Common
{
	public class EnterLevel : MenuCommand
	{
        public override void Execute(object data)
        {
            if (data is string levelName)
            {
                GameObserver.SwitchScene(levelName);
            }
            else
            {
                GameLogger.AddMessage("Got wrong command data. Please check source.", GameLogger.LogType.Error);
            }
        }
    }
}