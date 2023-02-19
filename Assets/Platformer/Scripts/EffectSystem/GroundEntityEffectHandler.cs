using Platformer.CharacterSystem.Base;
using Platformer.EditorExtentions;
using UnityEngine;

namespace Platformer.EffectSystem
{
	public class GroundEntityEffectHandler : EntityEffectHandler
	{
		[SerializeField]
		private TrailRenderer _dashEffect;
		[SerializeField]
		private ParticleSystemWrapper _hardLandingEffect;

        protected override void OnEntityEvent(object sender, EnitityEventTypes e)
        {
            base.OnEntityEvent(sender, e);
            switch (e)
            {
                case EnitityEventTypes.DashStarted:
                    _dashEffect.enabled = true;
                    break;
                case EnitityEventTypes.DashEnded:
                    _dashEffect.enabled = false;
                    break;
                case EnitityEventTypes.Landing:
                    _hardLandingEffect.Play();
                    break;
            }
        }
    }
}