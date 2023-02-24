using Platformer.CharacterSystem.Movement.Base;
using UnityEngine;

namespace Platformer.CharacterSystem.Base
{
	public abstract class MoveableEntity : Entity
	{
		private CharacterMovement _movementController;
        private bool _handlingEnabled;

        public CharacterMovement MovementController => _movementController;

        public bool HandlingEnabled
        {
            get => _handlingEnabled;
            set
            {
                _handlingEnabled = value;
                _movementController.MovementEnabled = _handlingEnabled;
            }
        }

        protected override void Start()
        {
            base.Start();
            _movementController = gameObject.GetComponent<CharacterMovement>();
            _movementController.EventInvoked += OnMovementEventInvoked;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (_movementController != null)
            {
                _movementController.EventInvoked -= OnMovementEventInvoked;
            }
        }

        private void OnMovementEventInvoked(object sender, EntityEventTypes e) =>
            InvokeEntityEvent(e);

        protected override void Update()
        {
            UpdateRotation();
        }

        public override void NotifyRespawn()
        {
            base.NotifyRespawn();
            _movementController.Velocity = Vector3.zero;
        }

        protected virtual void UpdateRotation()
        {
            if (_movementController.HorizontalInput > 0f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (_movementController.HorizontalInput < 0f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
        }
    }
}