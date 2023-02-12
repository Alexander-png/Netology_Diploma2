using Platformer.GameCore;
using Platformer.PlayerSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Platformer.UI
{
	public class HealthBar : MonoBehaviour
	{
		[Inject]
		private GameSystem _gameSystem;
		[SerializeField]
		private TMP_Text _text;

		private Player _player;

		private void Start() =>
            _gameSystem.GameLoaded += OnGameLoaded;

        private void OnGameLoaded(object sender, System.EventArgs e)
        {
            _gameSystem.GameLoaded -= OnGameLoaded;
            _player = _gameSystem.GetPlayer();
        }

        private void LateUpdate()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            if (_player != null)
            {
                _text.text = $"x:{_player.CurrentHealth}";
            }
        }
    }
}