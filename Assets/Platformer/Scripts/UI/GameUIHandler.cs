using Platformer.GameCore;
using Platformer.UI.MenuSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Platformer.UI
{
	public class GameUIHandler : MonoBehaviour
	{
		[Inject]
		private GameSystem _gameSystem;

		[SerializeField, Space(15f)]
		private RectTransform _menuBackground;
		[SerializeField]
		private RectTransform _statsBar;
		[SerializeField]
		private RectTransform _interactionTooltip;
		[SerializeField]
		private MenuComponent _pauseMenu;
		[SerializeField]
		private ConversationWidget _conversationWidget;
		[SerializeField]
		private LevelCompleteMessage _levelCompleteMenu;
		[SerializeField]
		private LevelTimer _levelTimer;

		private bool _onConversation;

		private bool GamePaused
		{
			get => _gameSystem.GamePaused;
			set => _gameSystem.GamePaused = value;
		}

        private void Start()
        {
			OnConversationUIEnabledChanged(null, _onConversation);
			OnCurrentInteractionTriggerChanged(null, null);
        }

        private void OnEnable() 
		{
			_gameSystem.PauseStateChanged += OnPauseStateChanged;
            _gameSystem.ConversationUIEnabledChanged += OnConversationUIEnabledChanged;
            _gameSystem.ConversationPhraseChanged += OnConversationPhraseChanged;
            _gameSystem.CurrentTriggerChanged += OnCurrentInteractionTriggerChanged;
            _gameSystem.CurrentTriggerInteracted += OnCurrentTriggerPerformed;
            _gameSystem.LevelCompleted += OnLevelCompleted;
		}

        private void OnDisable()
        {
			_gameSystem.PauseStateChanged -= OnPauseStateChanged;
			_gameSystem.ConversationUIEnabledChanged -= OnConversationUIEnabledChanged;
			_gameSystem.ConversationPhraseChanged -= OnConversationPhraseChanged;
			_gameSystem.CurrentTriggerChanged -= OnCurrentInteractionTriggerChanged;
			_gameSystem.CurrentTriggerInteracted -= OnCurrentTriggerPerformed;
			_gameSystem.LevelCompleted -= OnLevelCompleted;
		}

        private void OnPauseSwitch(InputValue input) =>
			GamePaused = !GamePaused;

		private void OnPauseStateChanged(object sender, bool value)
        {
			_menuBackground.gameObject.SetActive(value);
			_pauseMenu.gameObject.SetActive(value);
			_statsBar.gameObject.SetActive(!value);
			_levelTimer.gameObject.SetActive(!value);
			_interactionTooltip.gameObject.SetActive(!value && _gameSystem.CanCurrentTriggerPerformed);
		}

        private void OnConversationPhraseChanged(object sender, string phraseId)
        {
			// TODO: here may be localization system call
			_conversationWidget.SetText(phraseId);
		}

        private void OnConversationUIEnabledChanged(object sender, bool enabled)
        {
			_onConversation = enabled;
			_conversationWidget.gameObject.SetActive(_onConversation);
			_statsBar.gameObject.SetActive(!_onConversation);
			_interactionTooltip.gameObject.SetActive(_gameSystem.CanCurrentTriggerPerformed);
        }

		private void OnLevelCompleted(object sender, EventArgs e)
		{
			_menuBackground.gameObject.SetActive(_gameSystem.IsLevelCompleted);
			_statsBar.gameObject.SetActive(false);
			_levelTimer.gameObject.SetActive(false);
			_levelCompleteMenu.gameObject.SetActive(_gameSystem.IsLevelCompleted);
			_levelCompleteMenu.UpdateDisplayedTime(_gameSystem.LevelTime);
		}

		private void OnCurrentInteractionTriggerChanged(object sender, EventArgs e) =>
			_interactionTooltip.gameObject.SetActive(_gameSystem.CanCurrentTriggerPerformed);

		private void OnCurrentTriggerPerformed(object sender, EventArgs e) =>
			_interactionTooltip.gameObject.SetActive(_gameSystem.CanCurrentTriggerPerformed);
	}
}