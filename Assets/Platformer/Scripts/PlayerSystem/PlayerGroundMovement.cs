using Platformer.CharacterSystem.Movement;
using UnityEngine;

namespace Platformer.PlayerSystem
{
	public class PlayerGroundMovement : GroundCharacterMovement
	{
		private PlayerInputListener _inputListener;
        private AudioSource _walkAudioSource;

        public override float HorizontalInput
        {
            get => base.HorizontalInput;
            set
            {
                base.HorizontalInput = value;
                UpdateWalkAudioSource();
            }
        }

        public override bool OnGround
        {
            get => base.OnGround;
            protected set
            {
                base.OnGround = value;
                UpdateWalkAudioSource();
            }
        }

        private void UpdateWalkAudioSource()
        {
            if (OnGround && HorizontalInput != 0 && !_walkAudioSource.isPlaying)
            {
                _walkAudioSource.Play();
            }
            else
            {
                _walkAudioSource.Stop();
            }
        }

        protected override void Start()
        {
            base.Start();
            _inputListener = gameObject.GetComponent<PlayerInputListener>();
            _walkAudioSource = gameObject.GetComponent<AudioSource>();
            _walkAudioSource.clip = _walkingSound;
        }

        protected override Vector2 CalclulateDashDirection()
        {
            var vector = _inputListener.GetRelativeMousePosition(transform.position).normalized;
            vector.y /= MovementStats.VerticalDashDelimeter;
            return vector;
        }

        protected override void OnDashStarted()
        {
            base.OnDashStarted();
            Physics.IgnoreLayerCollision(6, 7, true);
        }

        protected override void OnDashEnded()
        {
            base.OnDashEnded();
            Physics.IgnoreLayerCollision(6, 7, false);
        }

        public override void UpdateRotation()
        {
            Vector3 relativeMousePos = _inputListener.GetRelativeMousePosition(transform.position);
            float rotation = relativeMousePos.x > 0 ? 0 : 180;
            transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        }
    }
}