using Platformer.LevelEnvironment.Elements.Common;
using Platformer.Scriptable.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.CharacterSystem.Movement.Base
{
    public abstract class CharacterMovement : MonoBehaviour
	{
		[SerializeField]
		private MovementStats _defaultMovementStats;
        [SerializeField]
        private bool _movementEnabled = true;

		private Rigidbody _body;
        protected MovementStatsInfo MovementStats { get; private set; }

        private float _horizontalInput;
        private float _verticalInput;
        private float _dashInput;

        protected List<GameObject> _currentCollisions;

        public Rigidbody Body => _body;
        
        public float Acceleration => MovementStats.Acceleration;
        public float MaxSpeed => MovementStats.MaxSpeed;
        public float InAirDrag => MovementStats.InAirDrag;
        public float DashForce => MovementStats.DashForce;
        public float DashDuration => MovementStats.DashDuration;
        public float DashRechargeTime => MovementStats.DashRechargeTime;

        public bool OnGround { get; protected set; }

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

        protected virtual void Awake()
        {
            _currentCollisions = new List<GameObject>();
            MovementStats = _defaultMovementStats.GetData();
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

        public virtual void SetDashInput(float input) { }

        public virtual void AddStats(MovementStatsInfo stats) =>
            MovementStats += stats;

        public virtual void RemoveStats(MovementStatsInfo stats) =>
            MovementStats -= stats;

        protected virtual void ResetState() 
        {
            HorizontalInput = 0f;
            VerticalInput = 0f;
            StopAllCoroutines();
        }
    }
}