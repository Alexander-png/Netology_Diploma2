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

		private void Start()
        {
            _player = _gameSystem.GetPlayer();
        }

        private void LateUpdate()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            _text.text = $"x:{_player.CurrentHealth}";
        }
    }
}