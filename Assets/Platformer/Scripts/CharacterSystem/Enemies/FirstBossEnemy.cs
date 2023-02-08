using Platformer.PlayerSystem;
using Platformer.Scriptable.Characters.AIConfig;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
    public class FirstBossEnemy : PatrolEnemy
    {
        [SerializeField]
        private ViewFieldConfig _viewFieldConfig;

        protected override void UpdateBehaviour()
        {
            // TODO: do all raycasts in fixed update
            if (!CheckCanMove() && !_pursuingPlayer)
            {
                _waitCoroutine = StartCoroutine(StopAndWait(_behaviourConfig.IdleTime));
                return;
            }

            if (_pursuingPlayer)
            {
                PursuitPlayer();
            }
            else
            {
                Patrol();
            }
        }

        protected override void FixedUpdateBehaviour()
        {
            CheckPlayerNearby();
        }

        protected override void CheckPlayerNearby()
        {
            Ray visualRay = GetHorizontalCensorRay(_viewFieldConfig.ViewOrigin);

            Physics.Raycast(visualRay, out RaycastHit frontHit, _viewFieldConfig.FrontViewRange);
            Physics.Raycast(visualRay.origin, -visualRay.direction, out RaycastHit behindHit, _viewFieldConfig.BehindViewRange);

            bool seePlayer = frontHit.transform?.TryGetComponent(out Player _) == true ||
                             behindHit.transform?.TryGetComponent(out Player _) == true;

            if (seePlayer)
            {
                OnPlayerNearby();
            }
            else
            {
                OnPlayerRanAway();
            }   
        }

        private void PursuitPlayer()
        {
            Vector3 playerPosition = _player.transform.position;
            Vector3 selfPosition = transform.position;
            if (playerPosition.x > selfPosition.x)
            {
                MovementController.HorizontalInput = 1f;
            }
            else if (playerPosition.x < selfPosition.x)
            {
                MovementController.HorizontalInput = -1f;
            }
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (_viewFieldConfig != null)
            {
                Gizmos.color = Color.magenta;
                Ray frontVisibility = GetHorizontalCensorRay(_viewFieldConfig.ViewOrigin);
                Gizmos.DrawRay(frontVisibility.origin, frontVisibility.direction * _viewFieldConfig.FrontViewRange);

                Gizmos.color = Color.cyan;
                Ray behindVisibility = GetHorizontalCensorRay(_viewFieldConfig.ViewOrigin);
                Gizmos.DrawRay(behindVisibility.origin, behindVisibility.direction * -_viewFieldConfig.BehindViewRange);
            }
            else
            {
                EditorExtentions.GameLogger.AddMessage("Please set view field config", EditorExtentions.GameLogger.LogType.Warning);
            }
        }
#endif
    }
}