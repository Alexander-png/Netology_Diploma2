using Newtonsoft.Json;
using Platformer.GameCore;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.UI.Settings
{
	public class SoundSettings : MonoBehaviour
	{
        [SerializeField]
		private Slider _volumeSlider;

        private GameSettingsManager.GameSettings _currentSettings;

        private void OnEnable()
        {
            string json = PlayerPrefs.GetString(GameSettingsManager.VolumeKey);
            _currentSettings = JsonConvert.DeserializeObject<GameSettingsManager.GameSettings>(json);
            _volumeSlider.value = _currentSettings.SoundMasterVolume;
        }

        private void OnDisable()
        {
            _currentSettings.SoundMasterVolume = _volumeSlider.value;
            string json = JsonConvert.SerializeObject(_currentSettings);
            PlayerPrefs.SetString(GameSettingsManager.VolumeKey, json);
            GameSettingsManager.InvokeSettingsChanged();
        }
    }
}