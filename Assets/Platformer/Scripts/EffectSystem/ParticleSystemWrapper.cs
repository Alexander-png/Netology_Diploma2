using UnityEngine;

namespace Platformer.EffectSystem
{
	public class ParticleSystemWrapper : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem[] _effects;

		public void Play()
        {
			foreach (var effect in _effects)
            {
				effect.Play();
            }
        }
	}
}