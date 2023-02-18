using Platformer.GameCore;
using Platformer.GameCore.Helpers;
using TMPro;
using UnityEngine;
using Zenject;

namespace Platformer.UI
{
	public class LevelTimer : MonoBehaviour
	{
		[Inject]
		private GameSystem _gameSystem;

        [SerializeField]
        private TMP_Text _timerValue;

        private void LateUpdate() =>
            _timerValue.text = TimeFormatter.GetFormattedTime(_gameSystem.LevelTime);
    }
}