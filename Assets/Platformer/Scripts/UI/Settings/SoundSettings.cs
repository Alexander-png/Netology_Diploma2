using Platformer.GameCore;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.UI.Settings
{
	public class SoundSettings : MonoBehaviour
	{
        [SerializeField]
		private Slider _volumeSlider;

        private GameSettingsObserver.GameSettings _currentSettings;

        private void OnEnable()
        {
            _currentSettings = GameSettingsObserver.GetSettings();
            _volumeSlider.value = _currentSettings.SoundMasterVolume;
        }

        private void OnDisable()
        {
            _currentSettings.SoundMasterVolume = _volumeSlider.value;
            GameSettingsObserver.SetSettings(_currentSettings);
        }
    }
}