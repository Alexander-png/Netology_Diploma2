using System;
using TMPro;
using UnityEngine;

namespace Platformer.UI
{
	public class LevelCompleteMessage : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _timeText;

		public void UpdateDisplayedTime(TimeSpan time) =>
			_timeText.text = time.ToString("mm\\:ss");
	}
}