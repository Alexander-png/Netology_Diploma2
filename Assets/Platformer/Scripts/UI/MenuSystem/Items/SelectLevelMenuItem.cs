using Platformer.GameCore;
using System;
using TMPro;
using UnityEngine;

namespace Platformer.UI.MenuSystem.Items
{
    public class SelectLevelMenuItem : MenuItem
    {
        [SerializeField]
        private TMP_Text _levelName;
        [SerializeField]
        private TMP_Text _bestTime;
        [SerializeField]
        private TMP_Text _goldTime;
        [SerializeField]
        private TMP_Text _silverTime;
        [SerializeField]
        private TMP_Text _bronzeTime;

        [SerializeField]
        private LevelData _levelData;
        public override object Data => _levelData;

        protected override void Start()
        {
            base.Start();
            FillVisual();
        }

        private void FillVisual()
        {
            _levelName.text = _levelData.Name;
            _bestTime.text = GetFormattedTime(_levelData.BestTime);
            _goldTime.text = GetFormattedTime(_levelData.GetTime(LevelCompletitionType.Gold));
            _silverTime.text = GetFormattedTime(_levelData.GetTime(LevelCompletitionType.Silver));
            _bronzeTime.text = GetFormattedTime(_levelData.GetTime(LevelCompletitionType.Bronze));
        }

        private string GetFormattedTime(float seconds) =>
            TimeSpan.FromSeconds(seconds).ToString("mm\\:ss");
    }
}