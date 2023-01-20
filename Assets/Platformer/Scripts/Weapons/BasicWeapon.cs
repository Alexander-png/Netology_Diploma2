using Platformer.CharacterSystem.Base;
using Platformer.Scriptable.LevelElements.Traps;
using UnityEngine;

namespace Platformer.Weapons
{
	public class BasicWeapon : MonoBehaviour
	{
        [SerializeField]
        protected DamageStats _damageStats;
        [SerializeField]
        protected Entity _owner;

        protected virtual void OnTriggerEnter(Collider other)
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