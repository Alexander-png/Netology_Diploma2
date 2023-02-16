using UnityEngine;

namespace Platformer.LevelEnvironment.Elements.Common
{
	public class PlatformMovement : MonoBehaviour
	{
		[SerializeField]
		private Vector3 _direciton;
        [SerializeField]
		private float _speed;

        public Vector3 Direciton
        {
            get => _direciton;
            set => _direciton = value;
        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        private void Update() =>
            Move();

        private void Move()
        {
            Vector3 position = transform.position;
            position += _speed * Time.deltaTime * _direciton.normalized;
            transform.position = position;
        }
    }
}