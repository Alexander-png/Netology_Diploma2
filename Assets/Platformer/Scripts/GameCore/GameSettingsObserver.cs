using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.GameCore
{
	public class GameSettingsObserver : MonoBehaviour
	{
        public struct GameSettings
        {
            public float SoundMasterVolume;
        }

        public const string VolumeKey = "GameSettings";
        public static event EventHandler SettingsChanged;

        private GameSettings _currentSettings;
        private List<AudioSource> _audioSources;

        private static string GetSettingsRaw() =>
            PlayerPrefs.GetString(VolumeKey);

        private static GameSettings CreateDefaultSettings() =>
            new GameSettings()
            {
                SoundMasterVolume = 0.5f,
            };

        private static void PrepareSettings()
        {
            string jsonSource = GetSettingsRaw();
            if (string.IsNullOrEmpty(jsonSource))
            {
                string json = JsonConvert.SerializeObject(CreateDefaultSettings());
                PlayerPrefs.SetString(VolumeKey, json);
            }
        }

        public static GameSettings GetSettings()
        {
            PrepareSettings();
            string jsonSource = GetSettingsRaw();
            return JsonConvert.DeserializeObject<GameSettings>(jsonSource);
        }

        public static void SetSettings(GameSettings settings)
        {
            string json = JsonConvert.SerializeObject(settings);
            PlayerPrefs.SetString(VolumeKey, json);
            InvokeSettingsChanged();
        }

        public static void InvokeSettingsChanged() =>
            SettingsChanged?.Invoke(null, null);

        private void Awake() =>
            _audioSources = new List<AudioSource>();

        private void OnEnable() =>
            SettingsChanged += OnSettingChanged;

        private void OnDisable()
        {
            SettingsChanged -= OnSettingChanged;
            _audioSources.Clear();
        }

        // Let all audiosources load by waiting 1 frame passed.
        private void Start() =>
            StartCoroutine(SkipFramesAndExecute(1, FindAudioSources, ApplySettings));

        private void OnSettingChanged(object sender, EventArgs e) =>
            ApplySettings();

        private void FindAudioSources() =>
            _audioSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());

        private void ApplySettings()
        {
            _currentSettings = GetSettings();

            foreach (AudioSource audioSource in _audioSources)
            {
                audioSource.volume = _currentSettings.SoundMasterVolume;
            }
        }

        private IEnumerator SkipFramesAndExecute(int value, params Action[] actions)
        {
            while (value > 0)
            {
                yield return null;
                value -= 1;
            }
            foreach (var action in actions)
            {
                action();
            }
        }
    }
}