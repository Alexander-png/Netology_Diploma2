using System;
using UnityEngine.SceneManagement;

namespace Platformer.UI.MenuSystem.Items
{
	public class EnterNextLevelMenuItem : MenuItem
	{
        public override object Data
        {
            get
            {
                string sceneName = SceneManager.GetActiveScene().name;
                int levelNumber = Convert.ToInt32(sceneName.Replace("Level", string.Empty));
                levelNumber += 1;
                string nextLevelName = $"Level{levelNumber}";
                bool isValid = SceneUtility.GetBuildIndexByScenePath(nextLevelName) != -1;
                if (!isValid)
                {
                    nextLevelName = "MenuScene";
                }
                return nextLevelName;
            }
        }
    }
}