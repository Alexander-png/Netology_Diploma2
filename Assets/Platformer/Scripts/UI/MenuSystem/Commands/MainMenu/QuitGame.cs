using Platformer.UI.MenuSystem.Commands.Base;

namespace Platformer.UI.MenuSystem.Commands.MainMenu
{
	public class QuitGame : MenuCommand
	{
        public override void Execute()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}