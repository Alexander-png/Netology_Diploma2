using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
    public class RaycastPatrolEnemy : Enemy
    {
        private enum MovementDirection : byte
        {
            Left = 0,
            Right = 1,
        }

        // TODO: move to config
        [SerializeField]
        private Vector3 _horizontalRayOrigin;
        [SerializeField]
        private float _horizontalRayLength;
        [SerializeField]
        private float _verticalRayLength;
        [SerializeField]
        private float _verticalRayOffset;

        private MovementDirection _direction;

        protected override void UpdateBehaviour()
        {
            if (CanMove())
            {
                StartCoroutine(StopAndWait(_idleTime));
                return;
            }

            //if (_pursuingPlayer)
            //{
            //    PursuitPlayer();
            //}
            //else
            if (!_inIdle)
            {
                Patrol();
            }
        }

        private bool CanMove()
        {
            Ray horizontalCensor = GetHorizontalRay();
            Ray verticalCensor = GetVerticalRay();
            bool isWallOnWay = Physics.Raycast(horizontalCensor, _horizontalRayLength);
            bool isHollowOnWay = !Physics.Raycast(verticalCensor, _verticalRayLength);
            return isWallOnWay || isHollowOnWay;
        }

        private void Patrol()
        {
            switch (_direction)
            {
                case MovementDirection.Left:
                    MovementController.MoveInput = -1f;
                    break;
                case MovementDirection.Right:
                    MovementController.MoveInput = 1f;
                    break;
            }
        }

        private void PursuitPlayer()
        {

        }

        private void ChangePatrolDirection() =>
            _direction = _direction == MovementDirection.Left ? MovementDirection.Right : MovementDirection.Left;

        private IEnumerator StopAndWait(float idleTime)
        {
            if (_inIdle)
            {
                yield break;
            }
            _inIdle = true;
            MovementController.MoveInput = 0;
            MovementController.Velocity = Vector3.zero;
            yield return new WaitForSeconds(idleTime);
            ChangePatrolDirection();
            Patrol();
            _inIdle = false;
        }

        private Ray GetHorizontalRay()
        {
            Vector3 startPoint = transform.TransformPoint(_horizontalRayOrigin);
            Vector3 endPoint = transform.rotation * new Vector3(_horizontalRayLength, 0, 0);
            return new Ray(startPoint, endPoint);
        }

        private Ray GetVerticalRay()
        {
            Vector3 origin = _horizontalRayOrigin;
            origin.x = _verticalRayOffset;
            Vector3 startPoint = transform.TransformPoint(origin);
            Vector3 endPoint = transform.rotation * new Vector3(0, -_verticalRayLength, 0);
            return new Ray(startPoint, endPoint);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Ray horz = GetHorizontalRay();
            Gizmos.DrawRay(horz.origin, horz.direction * _horizontalRayLength);
            Ray vert = GetVerticalRay();
            Gizmos.DrawRay(vert.origin, vert.direction * _verticalRayLength);
        }
#endif
    }
}