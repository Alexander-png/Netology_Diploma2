using Platformer.CharacterSystem.Base;
using Platformer.EditorExtentions;
using Platformer.Interaction;
using Platformer.PlayerSystem;
using Platformer.SkillSystem;
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

        [SerializeField]
        private string[] _defaultLevelSkillIds;

        private Player _player;
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
            _player.Died -= OnPlayerDied;
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

        public void SetPlayerHandlingEnabled(bool value) => _player.HandlingEnabled = value;

        public void AddSkillToPlayer(string skillId)
        {
            (_player as ISkillObservable).SkillObserver.AddSkill(skillId);
            GameLogger.AddMessage($"Given skill with id {skillId} to player.");
        }

        public bool CheckSkillAdded(string skillId) =>
            (_player as ISkillObservable).SkillObserver.CheckSkillAdded(skillId);

        public void OnItemCollected(IInventoryItem item) =>
            _player.Inventory.AddItem(item.ItemId);

        public void AddItemToPlayer(IInventoryItem item) =>
            _player.Inventory.AddItem(item.ItemId);

        public void RemoveItemFromPlayer(string itemId, int count = 1) =>
            _player.Inventory.RemoveItem(itemId, count);

        public bool CheckItemInInventory(string itemId, int count = 1) =>
            _player.Inventory.ContainsItem(itemId, count);

        public void SetConversationUIEnabled(bool value) =>
            ConversationUIEnabledChanged?.Invoke(this, value);

        public void ShowConversationPhrase(string phraseId) =>
            ConversationPhraseChanged?.Invoke(this, phraseId);

        public void OnLevelCompleted(string levelName)
        {
            _isLevelCompleted = true;
            SetPlayerHandlingEnabled(false);
            SaveSystem.OnLevelCompleted(levelName, _levelTime);
            _player.MovementController.Velocity = Vector3.zero;
            LevelCompleted?.Invoke(this, EventArgs.Empty);
        }

        public Player GetPlayer() => _player;

        private IEnumerator LoadedNotifier()
        {
            yield return null;
            _player = FindObjectOfType<Player>();
            if (_player == null)
            {
                GameLogger.AddMessage("No player found!", GameLogger.LogType.Fatal);
            }
            _player.Died += OnPlayerDied;
            SkillObserver skillObserver;

            _player.gameObject.TryGetComponent(out skillObserver);
            if (skillObserver != null)
            {
                skillObserver.AddSkill(_defaultLevelSkillIds, true);
                skillObserver.AddSkill(SaveSystem.GetRewardList());
            }

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
            _player = FindObjectOfType<Player>();
        }

        [ContextMenu("Clear save data")]
        private void ClearSaveData()
        {
            PlayerPrefs.DeleteKey(SaveSystem.SaveFileName);
        }
#endif
    }
}