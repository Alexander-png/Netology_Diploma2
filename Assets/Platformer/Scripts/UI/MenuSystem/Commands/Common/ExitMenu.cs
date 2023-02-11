using Platformer.UI.MenuSystem.Commands.Base;

namespace Platformer.UI.MenuSystem.Commands.Common
{
	public class ExitMenu : MenuCommand
	{
        public override void Execute(object data) =>
            ParentMenu.Exit();
    }
}