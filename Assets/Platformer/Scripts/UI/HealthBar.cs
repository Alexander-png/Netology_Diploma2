using Platformer.GameCore;
using Platformer.PlayerSystem;
using Platformer.UI.Helpers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Platformer.UI
{
	public class HealthBar : MonoBehaviour
	{
		[Inject]
		private GameSystem _gameSystem;

		private TMP_Text _text;
        private float _currentHealth;
        private Image _healthBarForeground;

        private Player _player;

		private void Start()
        {
            _gameSystem.GameLoaded += OnGameLoaded;
            _healthBarForeground = GetComponentInChildren<InWorldHeathBarForeground>().GetComponent<Image>();
            _text = GetComponentInChildren<TMP_Text>();
        }

        private void OnGameLoaded(object sender, EventArgs e)
        {
            _gameSystem.GameLoaded -= OnGameLoaded;
            _player = _gameSystem.GetPlayer();
        }

        private void LateUpdate() =>
            UpdateText();

        private void UpdateText()
        {
            if (_player == null)
            {
                return;
            }
            if (_currentHealth != _player.CurrentHealth)
            {
                _currentHealth = _player.CurrentHealth;
                _healthBarForeground.fillAmount = _currentHealth / _player.MaxHealth;
                _text.text = $"{_healthBarForeground.fillAmount * 100}%";
            }
        }
    }
}