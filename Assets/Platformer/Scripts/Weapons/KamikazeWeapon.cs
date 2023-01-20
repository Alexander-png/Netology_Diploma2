using Platformer.CharacterSystem.Base;
using UnityEngine;

namespace Platformer.Weapons
{
	public class KamikazeWeapon : EnemyWeapon
    {
        protected override void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out IDamagable target))
            {
                if (!target.Equals(_owner))
                {
                    Vector3 pushVector;
                    if (_owner is MoveableEntity moveable)
                    {
                        pushVector = moveable.MovementController.Velocity.normalized;
                        pushVector.y = Mathf.Abs(pushVector.y) * _damageStats.PushForce / 2f;
                    }
                    else
                    {
                        pushVector = (-other.attachedRigidbody.velocity + other.transform.up).normalized;
                    }
                    pushVector *= _damageStats.PushForce;
                    target.SetDamage(_damageStats.Damage, pushVector);
                    (_owner as IDamagable).SetDamage(float.MaxValue, Vector3.zero, true);
                }
            }
        }
    }
}