using Platformer.GameCore.Helpers;
using TMPro;
using UnityEngine;

namespace Platformer.UI
{
	public class LevelCompleteMessage : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _timeText;

		public void UpdateDisplayedTime(float time) =>
			_timeText.text = TimeFormatter.GetFormattedTime(time);
	}
}