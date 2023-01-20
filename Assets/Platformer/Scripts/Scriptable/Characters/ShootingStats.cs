using UnityEngine;

namespace Platformer.Scriptable.Characters
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Shooting Stats")]
	public class ShootingStats : ScriptableObject
	{
		[SerializeField]
		private float _reloadTime;
		[SerializeField]
		private float _shootDistance;
		[SerializeField]
		private float _startProjectileSpeed;

		public float ReloadTime => _reloadTime;
		public float ShootDistance => _shootDistance;
		public float StartProjectileSpeed => _startProjectileSpeed;
	}
}