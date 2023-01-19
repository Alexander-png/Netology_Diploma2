using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Platformer.GameCore
{
	public enum SceneTypes : byte
    {
		MainMenu = 0,
		Level1 = 1,
		Level2 = 2,
		Level3 = 3,
		Level4 = 4,
		Level5 = 5,
    }

	public static class GameObserver
	{
		private static Dictionary<SceneTypes, string> _sceneTypes;

		public static bool NewGameFlag;

		static GameObserver()
        {
			InitSceneTypes();
		}

		private static void InitSceneTypes()
        {
			_sceneTypes = new Dictionary<SceneTypes, string>()
			{
				{ SceneTypes.MainMenu, "MainMenu" },
				{ SceneTypes.Level1, "Level1" },
				{ SceneTypes.Level2, "Level2" },
				{ SceneTypes.Level3, "Level3" },
				{ SceneTypes.Level4, "Level4" },
				{ SceneTypes.Level5, "Level5" },
			};
        }

		public static void SwitchScene(SceneTypes sceneType) =>
			SceneManager.LoadScene(_sceneTypes[sceneType]);
	}
}