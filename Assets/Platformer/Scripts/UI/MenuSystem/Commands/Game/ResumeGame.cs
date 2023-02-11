using Platformer.UI.MenuSystem.Commands.Base;

namespace Platformer.UI.MenuSystem.Commands.Game
{
	public class ResumeGame : MenuCommand
	{
        public override void Execute(object data)
        {
            _gameSystem.GamePaused = false;
        }
	}
}