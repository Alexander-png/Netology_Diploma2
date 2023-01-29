using Platformer.LevelEnvironment.Elements.Common;
using Platformer.Scriptable.Characters;
using UnityEngine;

namespace Platformer.CharacterSystem.Movement.Base
{
    public abstract class CharacterMovement : MonoBehaviour
	{
		[SerializeField]
		private MovementStats _defaultMovementStats;
		[SerializeField]
		private Rigidbody _body;
        [SerializeField]
        private bool _movementEnabled = true;

        private MovementStatsInfo _movementStats;
        private Vector3 _currentCollisionNormal;

        private float _horizontalInput;
        private float _verticalInput;
        private float _dashInput;
        public Rigidbody Body => _body;
        protected int JumpsLeft { get; set; }

        public bool CanJump => JumpsLeft > 0;
        public float Acceleration => _movementStats.Acceleration;
        public float MaxSpeed => _movementStats.MaxSpeed;
        public float JumpForce => _movementStats.GetJumpForce(JumpsLeft);
        public float MaxJumpForce => _movementStats.MaxJumpForce;
        public int JumpCountInRow => _movementStats.JumpCountInRow;
        public float ClimbForce => _movementStats.ClimbForce;
        public float DashForce => _movementStats.DashForce;
        public float DashDuration => _movementStats.DashDuration;
        public float DashRechargeTime => _movementStats.DashRechargeTime;

        public bool OnGround { get; protected set; }

        public Vector3 CurrentCollisionNormal
        {
            get => _currentCollisionNormal;
            protected set => _currentCollisionNormal = value;
        }

        public bool InAir => _currentCollisionNormal == Vector3.zero;

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
            _movementStats = _defaultMovementStats.GetData();
            _currentCollisionNormal = Vector3.zero;
        }

        protected virtual void OnDisable()
        {
            ResetState();
        }

        protected virtual void OnCollisionEnter(Collision collision) 
        {
            var newNormal = collision.GetContact(0).normal;
            CurrentCollisionNormal = newNormal;
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Platform _))
            {
                _currentCollisionNormal = Vector3.zero;
            }
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

        public virtual void SetDashInput(float input) { }

        public virtual void AddStats(MovementStatsInfo stats) =>
            _movementStats += stats;

        public virtual void RemoveStats(MovementStatsInfo stats) =>
            _movementStats -= stats;

        protected virtual void ResetState() { }
    }
}