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

    public enum LevelCompletitionType : byte
    {
        NotCompleted = 0,
        Bronze = 1,
        Silver = 2,
        Gold = 3,
    }

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
            if (BestTime == float.NaN)
            {
                return LevelCompletitionType.NotCompleted;
            }

            KeyValuePair<float, LevelCompletitionType> res = StatusDict.First();
            foreach (var pair in StatusDict)
            {
                if (BestTime > pair.Key)
                {
                    res = pair;
                }
            }
            return res.Value;
        }

        private bool GetRewardData(LevelCompletitionType type, out RewardData result) =>
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

        public string GetRewardId(LevelCompletitionType type)
        {
            GetRewardData(type, out RewardData result);
            return result.Id;
        }

        public string GetRewardDescription(LevelCompletitionType type)
        {
            GetRewardData(type, out RewardData result);
            return result.Description;
        }

        public string[] GetRewardsForBestTime()
        {
            if (GetStatus() == LevelCompletitionType.NotCompleted)
            {
                return new string[0];
            }

            List<string> rewards = new List<string>();

            foreach (var pair in StatusDict)
            {
                if (BestTime < pair.Key)
                {
                    string id = GetRewardId(pair.Value);
                    if (!string.IsNullOrEmpty(id))
                    {
                        rewards.Add(id);
                    }
                }
            }
            return rewards.ToArray();
        }
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
                GameLogger.AddMessage("Failed to load save data. Waiting default data.", GameLogger.LogType.Warning);
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
            GameLogger.AddMessage("Default data setted.");
        }

        public static LevelData GetLevelData(string levelName)
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
            int index = _levelData.IndexOf(GetLevelData(levelName));
            if (time < _levelData[index].BestTime)
            {
                LevelData data = _levelData[index];
                data.BestTime = time;
                _levelData[index] = data;
                SaveLevelData();
            }
        }

        public static string[] GetRewardList()
        {
            List<string> rewards = new List<string>();
            foreach (LevelData data in _levelData)
            {
                rewards.AddRange(data.GetRewardsForBestTime());
            }
            return rewards.ToArray();
        }
    }
}