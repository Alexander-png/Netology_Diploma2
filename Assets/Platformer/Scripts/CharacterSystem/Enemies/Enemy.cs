using Newtonsoft.Json.Linq;
using Platformer.CharacterSystem.Base;
using Platformer.GameCore;
using Platformer.PlayerSystem;
using Platformer.Scriptable.Characters;
using System;
using UnityEngine;
using Zenject;

namespace Platformer.CharacterSystem.Enemies
{
	public abstract class Enemy : MoveableCharacter, IDamagableCharacter, ISaveable
    {
		[Inject]
		protected GameSystem _gameSystem;

        // TODO: better move these fields to scriptable object
        [SerializeField]
        protected float _idleTime;
        [SerializeField]
        protected float _attackRadius;
        [SerializeField]
        protected float _playerHeightDiffToJump;
        [SerializeField]
        protected float _closeToPlayerDistance;
        
        protected float _currentHealth;
        protected float _maxHealth;

        protected Player _player;

        protected bool _behaviourEnabled;
        protected bool _inIdle;
        protected bool _pursuingPlayer = false;

        public event EventHandler Died;

        protected class EnemyData : CharacterData
        {
            public bool AttackingPlayer;
            public bool InIdle;
        }

        public float CurrentHealth => _currentHealth;

        protected override void Start()
        {
            _gameSystem.RegisterSaveableObject(this);

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

        protected override void SetDefaultParameters(DefaultCharacterStats stats)
        {
            base.SetDefaultParameters(stats);
            _maxHealth = stats.MaxHealth;
            _currentHealth = _maxHealth;
        }

        protected virtual void UpdateBehaviour() { }
        protected virtual void FixedUpdateBehaviour() { }

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

        public virtual object GetData() => new EnemyData()
        {
            Name = gameObject.name,
            Side = Side,
            RawPosition = new CharacterData.Position3
            {
                x = transform.position.x,
                y = transform.position.y,
                z = transform.position.z,
            },
            CurrentHealth = CurrentHealth,
            AttackingPlayer = _pursuingPlayer,
            InIdle = _inIdle,
        };

        public virtual bool SetData(object data)
        {
            EnemyData dataToSet = data as EnemyData;
            if (!ValidateData(dataToSet))
            {
                return false;
            }

            Side = dataToSet.Side;
            transform.position = dataToSet.GetPositionAsVector3();
            _currentHealth = dataToSet.CurrentHealth;
            _pursuingPlayer = dataToSet.AttackingPlayer;
            _inIdle = dataToSet.InIdle;
            
            return true;
        }

        public virtual bool SetData(JObject data) => 
            SetData(data.ToObject<EnemyData>());

        public virtual void OnPlayerNearby()
        {
            _inIdle = false;
            _pursuingPlayer = true;
        }

        public virtual void OnPlayerRanAway() =>
            _pursuingPlayer = false;
    }
}