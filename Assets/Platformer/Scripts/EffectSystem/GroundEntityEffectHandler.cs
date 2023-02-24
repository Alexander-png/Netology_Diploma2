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

        protected override void OnEntityEvent(object sender, EntityEventTypes e)
        {
            base.OnEntityEvent(sender, e);
            switch (e)
            {
                case EntityEventTypes.DashStarted:
                    _dashEffect.enabled = true;
                    break;
                case EntityEventTypes.DashEnded:
                    _dashEffect.enabled = false;
                    break;
                case EntityEventTypes.Landing:
                    _hardLandingEffect.Play();
                    break;
            }
        }
    }
}