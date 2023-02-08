using Platformer.CharacterSystem.Attacking;
using Platformer.EditorExtentions;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
	public class StationaryShooter : StationaryEnemy
	{
        [SerializeField, ReadOnly]
        private DistantAttacker _attacker;

        protected override void Start()
        {
            base.Start();
            FindAttacker();
        }

        private void FindAttacker()
        {
            if (_attacker != null)
            {
                return;
            }
            _attacker = GetComponentInChildren<DistantAttacker>();
        }

        protected override void FixedUpdate() =>
            UpdateBehaviour();

        private void UpdateBehaviour()
        {
            if (CheckPlayerNearby())
            {
                _attacker.OnMainAttackPressed();
            }
        }

        private bool CheckPlayerNearby()
        {
            Ray visual = GetViewRay();
            // TODO: get view distance from something else but not from weapon
            return Physics.Raycast(visual, _attacker.GetShootDistance(), PlayerLayer);
        }

        private Ray GetCensorRay(Vector3 origin)
        {
            Vector3 startPoint = transform.TransformPoint(origin);
            Vector3 endPoint = transform.rotation * new Vector3(0, 1, 0);
            return new Ray(startPoint, endPoint);
        }

        private Ray GetViewRay() => GetCensorRay(_attacker.GetProjectileSpawnPointPosition());

#if UNITY_EDITOR
        private void OnValidate() =>
            FindAttacker();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            if (_attacker != null)
            {
                Ray horz = GetViewRay();
                Gizmos.DrawRay(horz.origin, horz.direction * _attacker.GetShootDistance());
            }
            else
            {
                GameLogger.AddMessage("Please set shoot stats and projectile spawn point", EditorExtentions.GameLogger.LogType.Warning);
            }
        }
#endif
    }
}