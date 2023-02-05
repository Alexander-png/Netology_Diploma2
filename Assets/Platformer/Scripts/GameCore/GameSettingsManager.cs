using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.GameCore
{
	public class GameSettingsManager : MonoBehaviour
	{
        public const string VolumeKey = "GameSettings";
        public static event EventHandler SettingsChanged;

        public static void InvokeSettingsChanged()
        {
            SettingsChanged?.Invoke(null, null);
        }

        public struct GameSettings
        {
            public float SoundMasterVolume;
        }

        private GameSettings _currentSettings;
        private List<AudioSource> _audioSources;

        private void Awake()
        {
            PrepareSettings();
            _audioSources = new List<AudioSource>();
        }

        private void OnEnable() =>
            SettingsChanged += OnSettingChanged;

        private void OnDisable() =>
            SettingsChanged -= OnSettingChanged;

        // Let all audiosources load by waiting 1 frame passed.
        private void Start() =>
            StartCoroutine(SkipFramesAndExecute(1, FindAudioSources, ApplySettings));

        private void PrepareSettings()
        {
            string jsonSource = GetSettingsInJson();
            if (string.IsNullOrEmpty(jsonSource))
            {
                string json = JsonConvert.SerializeObject(CreateDefaultSettings());
                PlayerPrefs.SetString(VolumeKey, json);
            }
        }

        private string GetSettingsInJson() =>
            PlayerPrefs.GetString(VolumeKey);

        private void OnSettingChanged(object sender, EventArgs e) =>
            ApplySettings();

        private void FindAudioSources() =>
            _audioSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());

        private void ApplySettings()
        {
            string jsonSource = GetSettingsInJson();
            _currentSettings = JsonConvert.DeserializeObject<GameSettings>(jsonSource);

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

        private GameSettings CreateDefaultSettings() =>
            new GameSettings()
            {
                SoundMasterVolume = 0.5f,
            };
    }
}