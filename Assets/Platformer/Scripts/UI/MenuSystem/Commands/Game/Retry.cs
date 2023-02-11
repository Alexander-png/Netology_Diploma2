using Platformer.EditorExtentions;
using Platformer.UI.MenuSystem.Commands.Base;

namespace Platformer.UI.MenuSystem.Commands.Game
{
	public class Retry : MenuCommand
	{
        public override void Execute(object data)
        {
            GameLogger.AddMessage("TODO: retry");
        }
    }
}