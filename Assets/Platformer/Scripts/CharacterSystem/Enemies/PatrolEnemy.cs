using Platformer.Scriptable.Characters.AIConfig;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
	public abstract class PatrolEnemy : MoveableEnemy
	{
        [SerializeField]
        private ObstacleDetectorConfig _detectorConfig;

        private enum MovementDirection : byte
        {
            Left = 0,
            Right = 1,
        }

        // TODO: implement point patroling 
        private enum PatrolMode : byte
        {
            Raycast = 0,
            Point = 1,
        }

        private MovementDirection _direction;
        protected Coroutine _waitCoroutine;

        protected virtual void Patrol()
        {
            switch (_direction)
            {
                case MovementDirection.Left:
                    MovementController.HorizontalInput = -1f;
                    break;
                case MovementDirection.Right:
                    MovementController.HorizontalInput = 1f;
                    break;
            }
        }

        protected virtual void ChangePatrolDirection() =>
            _direction = _direction == MovementDirection.Left ? MovementDirection.Right : MovementDirection.Left;

        public override void OnPlayerNearby()
        {
            base.OnPlayerNearby();
            if (_waitCoroutine != null)
            {
                StopCoroutine(_waitCoroutine);
            }
        }

        protected virtual bool CheckCanMove()
        {
            Ray horizontalCensor = GetHorizontalCensorRay(_detectorConfig.HorizontalSensorOrigin);
            Ray verticalCensor = GetVerticalCensorRay(_detectorConfig.HorizontalSensorOrigin);
            bool wallOnWay = Physics.Raycast(horizontalCensor, _detectorConfig.HorizontalSensorLength);
            bool HollowOnWay = Physics.Raycast(verticalCensor, _detectorConfig.VerticalSensorLength);
            return !wallOnWay && HollowOnWay;
        }

        protected virtual Ray GetHorizontalCensorRay(Vector3 origin)
        {
            Vector3 startPoint = transform.TransformPoint(origin);
            Vector3 endPoint = transform.rotation * new Vector3(1, 0, 0);
            return new Ray(startPoint, endPoint);
        }

        protected virtual Ray GetVerticalCensorRay(Vector3 origin)
        {
            origin.x = _detectorConfig.VerticalSensorOffset;
            Vector3 startPoint = transform.TransformPoint(origin);
            Vector3 endPoint = transform.rotation * new Vector3(0, -1, 0);
            return new Ray(startPoint, endPoint);
        }

        protected virtual IEnumerator StopAndWait(float idleTime)
        {
            if (_inIdle)
            {
                yield break;
            }
            _inIdle = true;
            MovementController.HorizontalInput = 0;
            MovementController.Velocity = Vector3.zero;
            yield return new WaitForSeconds(idleTime);
            if (!_pursuingPlayer)
            {
                ChangePatrolDirection();
                Patrol();
            }
            _waitCoroutine = null;
            _inIdle = false;
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
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
        }
#endif
    }
}