using Platformer.CharacterSystem.Base;
using UnityEngine;

namespace Platformer.Weapons
{
	public abstract class EnemyWeapon : BasicWeapon
	{
        protected override void OnTriggerEnter(Collider other) { }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out IDamagable target))
            {
                if (!target.Equals(_owner))
                {
                    Vector3 pushVector = (-other.attachedRigidbody.velocity + other.transform.up).normalized;
                    pushVector *= _damageStats.PushForce;
                    target.SetDamage(_damageStats.Damage, pushVector);
                }
            }
        }
    }
}