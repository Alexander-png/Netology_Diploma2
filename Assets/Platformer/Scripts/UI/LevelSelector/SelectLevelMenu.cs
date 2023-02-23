using Platformer.GameCore;
using Platformer.UI.MenuSystem;
using System.Collections.Generic;

namespace Platformer.UI.LevelSelector
{
	public class SelectLevelMenu : MenuComponent
	{
        protected override void Initialize()
        {
            base.Initialize();

            if (SaveSystem.NeedDefaultData)
            {
                List<LevelData> data = new List<LevelData>();
                foreach (var item in MenuItems)
                {
                    if (item.Data != null)
                    {
                        data.Add((LevelData)item.Data);
                    }
                }
                SaveSystem.SetDefaultData(data);
            }
        }
    }
}