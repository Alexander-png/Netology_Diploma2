using Platformer.CharacterSystem.Base;
using Platformer.EditorExtentions;
using Platformer.Interaction;
using Platformer.PlayerSystem;
using Platformer.Scriptable.Skills.Containers;
using System;
using System.Collections;
using UnityEngine;

namespace Platformer.GameCore
{
    public class GameSystem : MonoBehaviour
    {
        [SerializeField]
        private bool _gamePaused;
        [SerializeField]
        private float _levelReloadTime = 2f;

        private Player _playerCharacter;
        private bool _isLevelCompleted;
        private float _levelTime;
        private InteractableTrigger _currentInteractable;

        public bool GamePaused
        {
            get => _gamePaused;
            set
            {
                _gamePaused = value;
                PauseStateChanged?.Invoke(this, value);
            }
        }

        // Placed here just for notifying game UI about interactions
        public InteractableTrigger CurrentTrigger
        {
            get => _currentInteractable;
            set
            {
                if (_currentInteractable != null)
                {
                    _currentInteractable.Interacted -= CurrentTriggerInteracted;
                }
                _currentInteractable = value;
                if (_currentInteractable != null)
                {
                    _currentInteractable.Interacted += CurrentTriggerInteracted;
                }
                CurrentTriggerChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool CanCurrentTriggerPerformed => CurrentTrigger != null && CurrentTrigger.CanInteract;
        public bool IsLevelCompleted => _isLevelCompleted;
        public float LevelTime => _levelTime;

        public event EventHandler PlayerRespawned;
        public event EventHandler<bool> PauseStateChanged;
        public event EventHandler GameLoaded;
        public event EventHandler<bool> ConversationUIEnabledChanged;
        public event EventHandler<string> ConversationPhraseChanged;
        public event EventHandler CurrentTriggerChanged;
        public event EventHandler CurrentTriggerInteracted;
        public event EventHandler LevelCompleted;

        private void Start()
        {
            GamePaused = GamePaused;
            StartCoroutine(LoadedNotifier());
        }



        private void OnPlayerDied(object sender, EventArgs e)
        {
            _playerCharacter.Died -= OnPlayerDied;
            StartCoroutine(ReloadLevelCoroutine());
        }

        private void OnPauseStateChanged(object sender, bool e)
        {
            Time.timeScale = _gamePaused ? 0f : 1f;
        }

        private void OnEnable()
        {
            PauseStateChanged += OnPauseStateChanged;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            PauseStateChanged -= OnPauseStateChanged;
        }

        private void Update() =>
            _levelTime += Time.deltaTime;

        public void InvokePlayerRespawned() =>
            PlayerRespawned?.Invoke(this, EventArgs.Empty);

        private void OnAreaShowed(object sender, EventArgs e) =>
            SetPlayerHandlingEnabled(true);

        public void SetPlayerHandlingEnabled(bool value) => _playerCharacter.HandlingEnabled = value;

        public void AddSkillToPlayer(string skillId)
        {
            (_playerCharacter as ISkillObservable).SkillObserver.AddSkill(skillId);
            GameLogger.AddMessage($"Given skill with id {skillId} to player.");
        }

        public bool CheckSkillAdded(string skillId) =>
            (_playerCharacter as ISkillObservable).SkillObserver.CheckSkillAdded(skillId);

        public void OnItemCollected(IInventoryItem item) =>
            _playerCharacter.Inventory.AddItem(item.ItemId);

        public void AddItemToPlayer(IInventoryItem item) =>
            _playerCharacter.Inventory.AddItem(item.ItemId);

        public void RemoveItemFromPlayer(string itemId, int count = 1) =>
            _playerCharacter.Inventory.RemoveItem(itemId, count);

        public bool CheckItemInInventory(string itemId, int count = 1) =>
            _playerCharacter.Inventory.ContainsItem(itemId, count);

        public void SetConversationUIEnabled(bool value) =>
            ConversationUIEnabledChanged?.Invoke(this, value);

        public void ShowConversationPhrase(string phraseId) =>
            ConversationPhraseChanged?.Invoke(this, phraseId);

        public void OnLevelCompleted(string levelName)
        {
            _isLevelCompleted = true;
            SetPlayerHandlingEnabled(false);
            SaveSystem.OnLevelCompleted(levelName, _levelTime);
            _playerCharacter.MovementController.Velocity = Vector3.zero;
            LevelCompleted?.Invoke(this, EventArgs.Empty);
        }

        public Player GetPlayer() => _playerCharacter;

        private IEnumerator LoadedNotifier()
        {
            yield return null;
            _playerCharacter = FindObjectOfType<Player>();
            if (_playerCharacter == null)
            {
                GameLogger.AddMessage("No player found!", GameLogger.LogType.Fatal);
            }
            _playerCharacter.Died += OnPlayerDied;
            GameLoaded?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerator ReloadLevelCoroutine()
        {
            yield return new WaitForSeconds(_levelReloadTime);
            GameObserver.ReloadCurrentScene();
        }

#if UNITY_EDITOR
        [ContextMenu("Fill fields")]
        private void FindPlayerOnScene()
        {
            _playerCharacter = FindObjectOfType<Player>();
        }
#endif
    }
}