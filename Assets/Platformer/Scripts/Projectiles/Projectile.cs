using Platformer.GameCore;
using UnityEngine;

namespace Platformer.Projectiles
{
	public abstract class Projectile : MonoBehaviour
	{
		[SerializeField]
		private AudioClip _explosionSound;

		public abstract void SetSpeed(float value);

		protected virtual void Destroy()
        {
			if (_explosionSound != null)
            {
				AudioSource.PlayClipAtPoint(_explosionSound, gameObject.transform.position, GameSettingsObserver.GetSoundVolume());
            }
			Destroy(gameObject);
		}
	}
}