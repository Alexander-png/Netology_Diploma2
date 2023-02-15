using UnityEngine;

namespace Platformer.LevelEnvironment.Elements.Common
{
	public class PlatformMovement : MonoBehaviour
	{
		[SerializeField]
		private Vector3 _direciton;
        [SerializeField]
		private float _speed;

        private void Update() =>
            Move();

        private void Move()
        {
            Vector3 position = transform.position;
            position += _direciton * _speed * Time.deltaTime;
            transform.position = position;
        }
    }
}