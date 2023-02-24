using Platformer.CharacterSystem.Base;
using Platformer.EditorExtentions;
using UnityEngine;

namespace Platformer.EffectSystem
{
	public class EntityEffectHandler : EffectHandler
	{
        [SerializeField]
        private ParticleSystemWrapper _healEffect;

        private Entity _targetEntity;
        protected Entity TargetEntity => _targetEntity;

        protected virtual void Start()
        {
            _targetEntity = GetComponentInParent<Entity>();
            TargetEntity.EventInvoked += OnEntityEvent;
        }

        private void OnDisable() =>
            TargetEntity.EventInvoked -= OnEntityEvent;

        protected virtual void OnEntityEvent(object sender, EntityEventTypes e)
        {
            switch (e)
            {
                case EntityEventTypes.Heal:
                    _healEffect.Play();
                    break;
                case EntityEventTypes.Damage:
                    GameLogger.AddMessage("Damage");
                    break;
                case EntityEventTypes.Death:
                    GameLogger.AddMessage("Death");
                    break;
                case EntityEventTypes.Respawn:
                    GameLogger.AddMessage("Respawn");
                    break;
            }
        }
    }
}