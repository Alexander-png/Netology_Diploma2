using Platformer.CharacterSystem.Base;
using Platformer.EditorExtentions;
using Platformer.Weapons;
using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public class DistantAttacker : Attacker
	{
        [SerializeField, ReadOnly]
		protected DistantWeapon _currentWeapon;

        private bool _attackEventInvoked;

#if UNITY_EDITOR
        private void OnValidate() =>
            FindWeapon();
#endif

        protected virtual void Start() =>
            FindWeapon();

        private void FindWeapon()
        {
            if (_currentWeapon == null)
            {
                _currentWeapon = GetComponentInChildren<DistantWeapon>();
            }
        }

        public Vector3 GetProjectileSpawnPointPosition() =>
			_currentWeapon.ProjectileSpawnPosition;

		public float GetShootDistance() =>
			_currentWeapon.ShootDistance;

        public override void OnMainAttackPressed()
        {
            if (_currentWeapon.CanShoot && !_attackEventInvoked)
            {
                InvokeAttackerEvent(EntityEventTypes.Attack);
            }
        }

        public override void OnEventProcessed(EntityEventTypes e)
        {
            base.OnEventProcessed(e);
            if (e == EntityEventTypes.Attack)
            {
                _currentWeapon.Shoot();
                _attackEventInvoked = false;
            }
        }
    }
}