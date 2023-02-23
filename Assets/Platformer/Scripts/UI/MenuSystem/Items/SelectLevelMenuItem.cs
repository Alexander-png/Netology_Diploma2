using Platformer.GameCore;
using Platformer.GameCore.Helpers;
using Platformer.UI.MenuSystem.Items;
using TMPro;
using UnityEngine;

namespace Platformer.UI.LevelSelector
{
    public class SelectLevelMenuItem : MenuItem
    {
        [SerializeField]
        private TMP_Text _levelName;
        [SerializeField]
        private TMP_Text _bestTime;

        [SerializeField]
        private RewardTable _goldReward;
        [SerializeField]
        private RewardTable _silverReward;
        [SerializeField]
        private RewardTable _bronzeReward;

        [SerializeField]
        private LevelData _levelData;
        public override object Data
        {
            get => _levelData;
            set
            {
                _levelData = (LevelData)value;
                FillVisual();
            }
        }

        private void FillVisual()
        {
            _levelData = SaveSystem.GetLevelData(_levelData.Name);

            _levelName.text = _levelData.Name;
            _bestTime.text = TimeFormatter.GetFormattedTime(_levelData.BestTime);

            UpdateData(_goldReward, LevelCompletitionType.Gold);
            UpdateData(_silverReward, LevelCompletitionType.Silver);
            UpdateData(_bronzeReward, LevelCompletitionType.Bronze);
        }

        private void UpdateData(RewardTable table, LevelCompletitionType type) =>
            table.UpdateData(type, _levelData.GetTime(type), _levelData.GetRewardDescription(type));
    }
}