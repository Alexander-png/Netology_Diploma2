using Platformer.GameCore;
using Platformer.GameCore.Helpers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.UI.LevelSelector
{
	public class RewardTable : MonoBehaviour
	{
		// TODO: better move this into config
		private Dictionary<LevelCompletitionType, Color32> _colors = new Dictionary<LevelCompletitionType, Color32>
		{
			{ LevelCompletitionType.Gold, new Color32(255, 203, 0, 255) },
			{ LevelCompletitionType.Silver, new Color32(173, 173, 173, 255) },
			{ LevelCompletitionType.Bronze, new Color32(140, 89, 0, 255) },
		};

        [SerializeField]
		private Image _mark;
		[SerializeField]
		private TMP_Text _time;
		[SerializeField]
		private TMP_Text _reward;

        public void UpdateData(LevelCompletitionType type, float time, string reward)
        {
			_mark.color = _colors[type];
			_time.text = TimeFormatter.GetFormattedTime(time);
			_reward.text = reward;
		}
    }
}