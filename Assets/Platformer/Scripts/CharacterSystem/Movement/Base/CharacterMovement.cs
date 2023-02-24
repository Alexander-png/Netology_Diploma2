using Platformer.CharacterSystem.Base;
using Platformer.LevelEnvironment.Elements.Common;
using Platformer.Scriptable.Skills.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.CharacterSystem.Movement.Base
{
    public abstract class CharacterMovement : MonoBehaviour
	{
        [SerializeField]
        private MovementSkillConfiguration _defaultMovementConfig;

        [SerializeField]
        private bool _movementEnabled = true;

		private Rigidbody _body;
        protected MovementSkillData MovementStats { get; private set; }

        private float _horizontalInput;
        private float _verticalInput;
        private float _dashInput;
        private Animator _entityAnimator;

        protected List<GameObject> _currentCollisions;

        public Rigidbody Body => _body;
        
        public float Acceleration => MovementStats.Acceleration;
        public float MaxSpeed => MovementStats.MaxSpeed;
        public float InAirDrag => MovementStats.InAirDrag;
        public float DashForce => MovementStats.DashForce;
        public float DashDuration => MovementStats.DashDuration;
        public float DashRechargeTime => MovementStats.DashRechargeTime;

        public bool OnGround { get; protected set; }
        public bool IsJumping { get; protected set; }
        public bool IsDashing { get; protected set; }

        public void SetAnimator(Animator entityAnimator) =>
            _entityAnimator = entityAnimator;

        public bool InAir => _currentCollisions.Count == 0;

        public bool MovementEnabled 
        {
            get => _movementEnabled;
            set => _movementEnabled = value;
        }

        public virtual float HorizontalInput
        {
            get => _horizontalInput;
            set => _horizontalInput = value;
        }

        public virtual float VerticalInput
        {
            get => _verticalInput;
            set => _verticalInput = value;
        }

        public virtual float DashInput
        {
            get => _dashInput;
            set => _dashInput = value;
        }

        public virtual Vector3 Velocity
        {
            get => _body.velocity;
            set => _body.velocity = value;
        }

        public event EventHandler<EntityEventTypes> EventInvoked;

        protected virtual void Awake()
        {
            _currentCollisions = new List<GameObject>();
            MovementStats = _defaultMovementConfig.GetData();
        }

        protected virtual void Start() =>
            _body = gameObject.GetComponent<Rigidbody>();

        protected virtual void OnDisable()
        {
            ResetState();
        }

        protected virtual void OnCollisionEnter(Collision collision) 
        {
            if (collision.gameObject.TryGetComponent(out Platform _))
            {
                _currentCollisions.Add(collision.gameObject);
            }
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Platform _))
            {
                _currentCollisions.Remove(collision.gameObject);
            }
        }

        protected virtual Vector2 CalclulateDashDirection() =>
            Vector2.zero;

        protected virtual void ResetState()
        {
            HorizontalInput = 0f;
            VerticalInput = 0f;
            StopAllCoroutines();
        }

        public virtual void StopImmediatly()
        {
            HorizontalInput = 0f;
            VerticalInput = 0f;
            Velocity = Vector3.zero;
        }

        public virtual void SetHorizontalInput(float input) =>
            HorizontalInput = input;

        public virtual void SetVerticalInput(float input) =>
            VerticalInput = input;

        public virtual void TriggerDash(float input) { }

        public virtual void AddSkill(MovementSkillData stats) =>
            MovementStats += stats;

        public virtual void RemoveSkill(MovementSkillData stats) =>
            MovementStats -= stats;

        public void SetAnimatorState(string name, float value)
        {
            if (_entityAnimator != null)
            {
                _entityAnimator.SetFloat(name, value);
            }
        }

        public void InvokeEntityEvent(EntityEventTypes e) =>
            EventInvoked?.Invoke(this, e);
    }
}