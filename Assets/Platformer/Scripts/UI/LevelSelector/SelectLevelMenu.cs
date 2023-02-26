using Platformer.GameCore;
using Platformer.UI.MenuSystem;
using System.Collections.Generic;

namespace Platformer.UI.LevelSelector
{
    // TODO: How to synchronize data in reward table and save data in right way?
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
                        var newData = (LevelData)item.Data;
                        newData.BestTime = float.MaxValue;
                        data.Add(newData);
                    }
                }
                SaveSystem.SetDefaultData(data);
            }
            foreach (var item in MenuItems)
            {
                if (item.Data != null)
                {
                    string levelName = ((LevelData)item.Data).Name;
                    item.Data = SaveSystem.GetLevelData(levelName);
                }
            }
        }
    }
}