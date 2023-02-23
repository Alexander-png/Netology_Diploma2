using Newtonsoft.Json;
using Platformer.EditorExtentions;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Platformer.GameCore
{
    [Serializable]
    public class TimeStatusDictionary : SerializableDictionaryBase<float, LevelCompletitionType> { }
    [Serializable]
    public class StatusRewardDictionary : SerializableDictionaryBase<LevelCompletitionType, RewardData> { }

    [Serializable]
    public struct RewardData
    {
        public string Id;
        public string Description;
    }

    [Serializable]
    public struct LevelData
    {
        public string Name;
        public float BestTime;

        public TimeStatusDictionary StatusDict;
        public StatusRewardDictionary RewardDict;

        private LevelCompletitionType GetStatus()
        {
            KeyValuePair<float, LevelCompletitionType> res = StatusDict.First();
            foreach (KeyValuePair<float, LevelCompletitionType> pair in StatusDict)
            {
                if (BestTime > pair.Key)
                {
                    res = pair;
                }
            }
            return res.Value;
        }

        private bool GetReward(LevelCompletitionType type, out RewardData result) =>
            RewardDict.TryGetValue(type, out result);

        public float GetTime(LevelCompletitionType status)
        {
            foreach (KeyValuePair<float, LevelCompletitionType> pair in StatusDict)
            {
                if (status == pair.Value)
                {
                    return pair.Key;
                }
            }
            return 0;
        }

        public void UpdateTime(float time) =>
            BestTime = time;

        public string GetRewardId(LevelCompletitionType type)
        {
            GetReward(type, out RewardData result);
            return result.Id;
        }

        public string GetRewardDescription(LevelCompletitionType type)
        {
            GetReward(type, out RewardData result);
            return result.Description;
        }

        public bool GetRewardBestTime(out RewardData reward) =>
            RewardDict.TryGetValue(GetStatus(), out reward);
    }

    public static class SaveSystem
	{
        private const string SaveFileName = "Save01";

        private static List<LevelData> _levelData;

        public static bool NeedDefaultData { get; private set; }

        static SaveSystem()
        {
            string jsonSource = PlayerPrefs.GetString(SaveFileName);
            if (string.IsNullOrEmpty(jsonSource))
            {
                NeedDefaultData = true;
                return;
            }
            try
            {
                _levelData = JsonConvert.DeserializeObject<List<LevelData>>(jsonSource);
            }
            catch
            {
                NeedDefaultData = true;
            }
        }

        private static bool CheckState()
        {
            if (NeedDefaultData)
            {
                GameLogger.AddMessage($"Save file not initialized.", GameLogger.LogType.Fatal);
                return false;
            }
            return true;
        }

        private static void SaveLevelData() =>
            PlayerPrefs.SetString(SaveFileName, JsonConvert.SerializeObject(_levelData, Formatting.Indented));

        public static void SetDefaultData(List<LevelData> data)
        {
            _levelData = new List<LevelData>(data);
            SaveLevelData();
            NeedDefaultData = false;
        }

        public static LevelData GetLevelInfo(string levelName)
        {
            if (!CheckState())
            {
                return new LevelData();
            }
            if (!GameObserver.CheckLevelExists(levelName))
            {
                GameLogger.AddMessage($"Level with name {levelName} does not exist.", GameLogger.LogType.Fatal);
                return new LevelData();
            }
            return _levelData.Find(l => l.Name == levelName);
        }

        public static void OnLevelCompleted(string levelName, float time)
        {
            if (!CheckState())
            {
                return;
            }
            int index = _levelData.IndexOf(GetLevelInfo(levelName));
            if (_levelData[index].BestTime < time)
            {
                LevelData data = _levelData[index];
                data.BestTime = time;
                _levelData[index] = data;
                SaveLevelData();
            }
        }
    }
}