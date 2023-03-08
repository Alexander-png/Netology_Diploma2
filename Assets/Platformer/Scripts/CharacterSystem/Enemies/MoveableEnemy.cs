using Platformer.CharacterSystem.Attacking;
using Platformer.CharacterSystem.Base;
using Platformer.EditorExtentions;
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
        // TODO: move to configuration class
        protected const int PlayerMask = 1 << 6;
        protected const int ObstacleMask = 1 << 3 | ~(1 << 7 | 1 << 8 | 1 << 10 | 1 << 11);

        [SerializeField]
        private AudioClip _damageSound;

        [Inject]
        protected GameSystem _gameSystem;
        [SerializeField]
        protected EnemyBehaviourConfig _behaviourConfig;

        private bool _dead;

        protected Attacker _attacker;
        protected float _currentHealth;
        protected Player _player;

        protected bool _behaviourEnabled;
        protected bool _inIdle;
        protected bool _pursuingPlayer = false;

        public event EventHandler Died;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _currentStats.MaxHealth;
        public bool CanBeDamaged => true;

        protected bool CloseToPlayer =>
            Vector3.Distance(transform.position, _player.transform.position) < _behaviourConfig.CloseToPlayerDistance;

        protected override void OnDisable()
        {
            if (_attacker != null)
            {
                _attacker.EventInvoked -= OnAttackerEvent;
            }
        }

        protected override void Start()
        {
            base.Start();
            _attacker = gameObject.GetComponentInChildren<Attacker>();
            if (_attacker == null)
            {
                GameLogger.AddMessage("No attacker found. Can not attack enemies", GameLogger.LogType.Error);
            }
            _attacker.EventInvoked += OnAttackerEvent;
             _gameSystem.GameLoaded += OnGameLoaded;
            SetBehaviourEnabled(true);
            _currentHealth = MaxHealth;
        }

        protected override void Update()
        {
            base.Update();
            if (!_behaviourEnabled)
            {
                return;
            }
            UpdateBehaviour();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            FixedUpdateBehaviour();
        }

        private void OnAttackerEvent(object sender, EntityEventTypes e) =>
            InvokeEntityEvent(e);

        private void OnGameLoaded(object sender, EventArgs e)
        {
            _gameSystem.GameLoaded -= OnGameLoaded;
            _player = _gameSystem.GetPlayer();
        }

        protected virtual void UpdateBehaviour() { }
        protected virtual void FixedUpdateBehaviour() { }
        protected virtual void CheckPlayerNearby() { }

        public bool SetBehaviourEnabled(bool value) =>
            _behaviourEnabled = value;

        public virtual void SetDamage(float damage, Vector3 pushVector, bool forced = false)
        {
            if (_currentHealth <= 0)
            {
                return;
            }

            SetBehaviourEnabled(false);
            MovementController.MovementEnabled = false;
            MovementController.Velocity = pushVector;

            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, MaxHealth);
            if (_currentHealth < 0.01f)
            {
                InvokeEntityEvent(EntityEventTypes.Death);
                GameLogger.AddMessage($"Enemy with name {gameObject.name} killed. You can implement respawn system");
            }
            else
            {
                InvokeEntityEvent(EntityEventTypes.Damage);
            }

            if (_damageSound != null)
            {
                AudioSource.PlayClipAtPoint(_damageSound, transform.position, GameSettingsObserver.GetSoundVolume());
            }
        }

        public override void InvokeEntityEvent(EntityEventTypes e)
        {
            base.InvokeEntityEvent(e);

            if (_dead)
            {
                return;
            }

            if (e == EntityEventTypes.Death)
            {
                _dead = true;
                Died?.Invoke(this, EventArgs.Empty);
            }
        }

        public override void OnEventProcessed(EntityEventTypes e)
        {
            switch (e)
            {
                case EntityEventTypes.Damage:
                    SetBehaviourEnabled(true);
                    MovementController.MovementEnabled = true;
                    InvokeEntityEvent(EntityEventTypes.ResetState);
                    break;
                case EntityEventTypes.Death:
                    gameObject.SetActive(false);
                    break;
            }
            MovementController.OnEventProcessed(e);
            _attacker.OnEventProcessed(e);
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

#if UNITY_EDITOR
        [ContextMenu("Invoke damage")]
        private void Damage()
        {
            SetDamage(0, Vector3.zero);
        }

        [ContextMenu("Invoke death")]
        private void Death()
        {
            SetDamage(_currentHealth, Vector3.zero);
        }

        protected virtual void OnDrawGizmos()
        {
            Color c = Color.cyan;
            c.a = 0.2f;
            Gizmos.color = c;
            Gizmos.DrawSphere(transform.position, _behaviourConfig.CloseToPlayerDistance);
        }
#endif
    }
}