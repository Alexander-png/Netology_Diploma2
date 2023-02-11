using Newtonsoft.Json;
using Platformer.EditorExtentions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.GameCore
{
    public static class SaveSystem
	{
        private const string SaveFileName = "Save01";

        public struct SaveData
        {
            public string LevelName;
            public LevelCompleteType Status;
            public TimeSpan Time;
        }

        private static List<SaveData> _levelData;

        static SaveSystem()
        {
            string jsonSource = PlayerPrefs.GetString(SaveFileName);
            if (string.IsNullOrEmpty(jsonSource))
            {
                SetDefaultData();
                return;
            }
            try
            {
                _levelData = JsonConvert.DeserializeObject<List<SaveData>>(jsonSource);
            }
            catch
            {
                SetDefaultData();
            }
        }

        private static void SetDefaultData()
        {
            List<string> levelNames = GameObserver.GetLevelNames();
            _levelData = new List<SaveData>(levelNames.Count);
            foreach (var levelName in levelNames)
            {
                _levelData.Add(new SaveData()
                {
                    LevelName = levelName,
                    Status = LevelCompleteType.NotCompleted,
                    Time = TimeSpan.Zero,
                });
            }
            PlayerPrefs.SetString(SaveFileName, JsonConvert.SerializeObject(_levelData));
        }

        public static SaveData GetLevelInfo(string levelName)
        {
            if (!GameObserver.CheckLevelExists(levelName))
            {
                GameLogger.AddMessage($"Level with name {levelName} does not exist.", GameLogger.LogType.Fatal);
                return new SaveData();
            }
            return _levelData.Find(l => l.LevelName == levelName);
        }
    }
}