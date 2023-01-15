using Platformer.UI.MenuSystem.Commands.Base;

namespace Platformer.UI.MenuSystem.Commands.Game
{
	public class SaveGame : MenuCommand
	{
        public override void Execute()
        {
            _gameSystem.SaveSystem.PerformSave(true);
        }
	}
}