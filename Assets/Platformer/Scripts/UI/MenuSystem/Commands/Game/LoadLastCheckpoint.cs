using Platformer.UI.MenuSystem.Commands.Base;

namespace Platformer.UI.MenuSystem.Commands.Game
{
	public class LoadLastCheckpoint : MenuCommand
	{
        public override void Execute()
        {
            _gameSystem.SaveSystem.LoadLastAutoSave();
            _gameSystem.GamePaused = false;
        }
	}
}