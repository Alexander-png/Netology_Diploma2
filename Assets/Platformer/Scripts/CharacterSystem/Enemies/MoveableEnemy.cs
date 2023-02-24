using Platformer.CharacterSystem.Attacking;
using Platformer.CharacterSystem.Base;
using Platformer.GameCore;
using Platformer.PlayerSystem;
using Platformer.Scriptable.EntityConfig;
using System;
using UnityEngine;
using Zenject;

namespace Platformer.CharacterSystem.Enemies
{
    public abstract class MoveableEnemy : MoveableEntity, IDamagable
    {
        protected const int PlayerLayer = 1 << 6;

        [Inject]
        protected GameSystem _gameSystem;
        [SerializeField]
        protected EnemyBehaviourConfig _behaviourConfig;

        protected Attacker _attacker;
        protected float _currentHealth;
        protected Player _player;

        protected bool _behaviourEnabled;
        protected bool _inIdle;
        protected bool _pursuingPlayer = false;

        public event EventHandler Died;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _currentStats.MaxHealth;

        protected override void Start()
        {
            base.Start();
            _attacker = gameObject.GetComponentInChildren<Attacker>();
            _gameSystem.GameLoaded += OnGameLoaded;

            MovementController.MovementEnabled = true;
            MovementController.SetAnimator(EntityAnimator);
            SetBehaviourEnabled(true);
            _currentHealth = MaxHealth;
        }

        private void OnGameLoaded(object sender, EventArgs e)
        {
            _gameSystem.GameLoaded -= OnGameLoaded;
            _player = _gameSystem.GetPlayer();
        }

        protected override void Update()
        {
            base.Update();
            UpdateBehaviour();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            FixedUpdateBehaviour();
        }

        protected virtual void UpdateBehaviour() { }
        protected virtual void FixedUpdateBehaviour() { }
        protected virtual void CheckPlayerNearby() { }

        protected void InvokeDiedEvent() => 
            Died?.Invoke(this, EventArgs.Empty);

        public bool SetBehaviourEnabled(bool value) =>
            _behaviourEnabled = value;

        public void SetDamage(float damage, Vector3 pushVector, bool forced = false)
        {
            if (_currentHealth <= 0)
            {
                return;
            }

            MovementController.Velocity = pushVector;
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, MaxHealth);
            if (_currentHealth < 0.01f)
            {
                InvokeDiedEvent();
                SetBehaviourEnabled(false);
                EditorExtentions.GameLogger.AddMessage($"Enemy with name {gameObject.name} killed. You can implement respawn system");
                gameObject.SetActive(false);
            }
        }

        public void Heal(float value) =>
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0, MaxHealth);

        public virtual void OnPlayerNearby()
        {
            if (_pursuingPlayer)
            {
                return;
            }
            _inIdle = false;
            _pursuingPlayer = true;
            _attacker.OnMainAttackPressed();
        }

        public virtual void OnPlayerRanAway()
        {
            if (!_pursuingPlayer)
            {
                return;
            }
            _pursuingPlayer = false;
            _attacker.OnAttackReleased();
        }
    }
}