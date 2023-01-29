using UnityEngine;

namespace Platformer.Projectiles
{
	public abstract class Projectile : MonoBehaviour
	{
		public abstract void SetSpeed(float value);
	}
}