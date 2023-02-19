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

        protected virtual void OnEntityEvent(object sender, EnitityEventTypes e)
        {
            switch (e)
            {
                case EnitityEventTypes.Heal:
                    _healEffect.Play();
                    break;
                case EnitityEventTypes.Damage:
                    GameLogger.AddMessage("Damage");
                    break;
                case EnitityEventTypes.Death:
                    GameLogger.AddMessage("Death");
                    break;
                case EnitityEventTypes.Respawn:
                    GameLogger.AddMessage("Respawn");
                    break;
            }
        }
    }
}