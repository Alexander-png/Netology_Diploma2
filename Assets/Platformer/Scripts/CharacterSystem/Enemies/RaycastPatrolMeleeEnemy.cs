using Platformer.PlayerSystem;
using Platformer.Scriptable.Characters.AIConfig;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
    public class RaycastPatrolMeleeEnemy : PatrolEnemy
    {
        [SerializeField]
        private ObstacleDetectorConfig _detectorConfig;
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

        private bool CheckCanMove()
        {
            Ray horizontalCensor = GetHorizontalCensorRay(_detectorConfig.HorizontalSensorOrigin);
            Ray verticalCensor = GetVerticalCensorRay(_detectorConfig.HorizontalSensorOrigin);
            bool wallOnWay = Physics.Raycast(horizontalCensor, _detectorConfig.HorizontalSensorLength);
            bool HollowOnWay = Physics.Raycast(verticalCensor, _detectorConfig.VerticalSensorLength);
            return !wallOnWay && HollowOnWay;
        }

        protected override void CheckPlayerNearby()
        {
            Ray frontVisual = GetHorizontalCensorRay(_viewFieldConfig.ViewOrigin);
            Ray behindVisual = GetHorizontalCensorRay(_viewFieldConfig.ViewOrigin);

            Physics.Raycast(frontVisual, out RaycastHit frontHit, _viewFieldConfig.FrontViewRange);
            Physics.Raycast(behindVisual.origin, -behindVisual.direction, out RaycastHit behindHit, _viewFieldConfig.BehindViewRange);

            bool seePlayer = frontHit.transform?.TryGetComponent<Player>(out _) == true || 
                             behindHit.transform?.TryGetComponent<Player>(out _) == true;

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

        private Ray GetHorizontalCensorRay(Vector3 origin)
        {
            Vector3 startPoint = transform.TransformPoint(origin);
            Vector3 endPoint = transform.rotation * new Vector3(1, 0, 0);
            return new Ray(startPoint, endPoint);
        }

        private Ray GetVerticalCensorRay(Vector3 origin)
        {
            origin.x = _detectorConfig.VerticalSensorOffset;
            Vector3 startPoint = transform.TransformPoint(origin);
            Vector3 endPoint = transform.rotation * new Vector3(0, -1, 0);
            return new Ray(startPoint, endPoint);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            if (_detectorConfig != null)
            {
                Ray horz = GetHorizontalCensorRay(_detectorConfig.HorizontalSensorOrigin);
                Gizmos.DrawRay(horz.origin, horz.direction * _detectorConfig.HorizontalSensorLength);
                Ray vert = GetVerticalCensorRay(_detectorConfig.HorizontalSensorOrigin);
                Gizmos.DrawRay(vert.origin, vert.direction * _detectorConfig.VerticalSensorLength);
            }
            else
            {
                EditorExtentions.GameLogger.AddMessage("Please set obstacle detection config", EditorExtentions.GameLogger.LogType.Warning);
            }

            Gizmos.color = Color.magenta;

            if (_viewFieldConfig != null)
            {
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