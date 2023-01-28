using Platformer.CharacterSystem.Attacking;
using Platformer.CharacterSystem.Base;
using Platformer.GameCore;
using Platformer.PlayerSystem;
using Platformer.Scriptable.Characters;
using System;
using UnityEngine;
using Zenject;

namespace Platformer.CharacterSystem.Enemies
{
    // TODO: fix enemy behaviour on pursuit player
    public abstract class MoveableEnemy : MoveableEntity, IDamagable
    {
        [Inject]
        protected GameSystem _gameSystem;

        [SerializeField]
        protected EnemyBehaviourConfig _behaviourConfig;
        [SerializeField]
        protected MeleeAttacker _attacker;

        protected float _currentHealth;
        protected float _maxHealth;

        protected Player _player;

        protected bool _behaviourEnabled;
        protected bool _inIdle;
        protected bool _pursuingPlayer = false;

        public event EventHandler Died;

        public float CurrentHealth => _currentHealth;

        protected override void Start()
        {
            _player = _gameSystem.GetPlayer();
            MovementController.MovementEnabled = true;
            SetBehaviourEnabled(true);
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

        protected override void SetDefaultParameters(CharacterStats stats)
        {
            base.SetDefaultParameters(stats);
            _maxHealth = stats.MaxHealth;
            _currentHealth = _maxHealth;
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
            MovementController.Velocity = pushVector;
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
            if (_currentHealth < 0.01f)
            {
                InvokeDiedEvent();
                SetBehaviourEnabled(false);
                EditorExtentions.GameLogger.AddMessage($"Enemy with name {gameObject.name} was. Killed. You can implement respawn system");
                gameObject.SetActive(false);
            }
        }

        public void Heal(float value) =>
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _maxHealth);

        public virtual void OnPlayerNearby()
        {
            if (_pursuingPlayer)
            {
                return;
            }
            _inIdle = false;
            _pursuingPlayer = true;
            _attacker.StartAttack();
        }

        public virtual void OnPlayerRanAway()
        {
            if (!_pursuingPlayer)
            {
                return;
            }
            _pursuingPlayer = false;
            _attacker.EndAttack();
        }
    }
}