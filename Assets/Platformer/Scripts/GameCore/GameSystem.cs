using Platformer.CharacterSystem.Base;
using Platformer.EditorExtentions;
using Platformer.Interaction;
using Platformer.PlayerSystem;
using Platformer.Scriptable.Skills.Containers;
using System;
using System.Collections;
using UnityEngine;

// TODO: find assets for all objects

namespace Platformer.GameCore
{
    public class GameSystem : MonoBehaviour
    {
        [SerializeField]
        private bool _gamePaused;
        [SerializeField]
        private Player _playerCharacter;

        [SerializeField, Space(15)]
        private MovementSkillContainer _playerMovementSkillContainer;

        private bool _isGameCompleted;
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

        public MovementSkillContainer PlayerMovementSkillContainer => _playerMovementSkillContainer;

        public bool CanCurrentTriggerPerformed => CurrentTrigger != null && CurrentTrigger.CanInteract;
        public bool IsGameCompleted => _isGameCompleted;

        public event EventHandler PlayerRespawned;
        public event EventHandler<bool> PauseStateChanged;
        public event EventHandler GameLoaded;
        public event EventHandler<bool> ConversationUIEnabledChanged;
        public event EventHandler<string> ConversationPhraseChanged;
        public event EventHandler CurrentTriggerChanged;
        public event EventHandler CurrentTriggerInteracted;
        public event EventHandler GameCompleted;


        private void Awake()
        {
            if (_playerCharacter == null)
            {
                GameLogger.AddMessage($"{nameof(GameSystem)}: no player character assigned!", GameLogger.LogType.Fatal);
            }
        }

        private void Start()
        {
            GamePaused = GamePaused;
            StartCoroutine(LoadedNotifier());
        }

        private IEnumerator LoadedNotifier()
        {
            yield return null;
            GameLoaded?.Invoke(this, EventArgs.Empty);
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

        public void NotifyGameCompleted()
        {
            _isGameCompleted = true;
            SetPlayerHandlingEnabled(false);
            _playerCharacter.MovementController.Velocity = Vector3.zero;
            GameCompleted?.Invoke(this, EventArgs.Empty);
        }

        public Player GetPlayer() => _playerCharacter;

#if UNITY_EDITOR
        [ContextMenu("Fill fields")]
        private void FindPlayerOnScene()
        {
            _playerCharacter = FindObjectOfType<Player>();
        }
#endif
    }
}