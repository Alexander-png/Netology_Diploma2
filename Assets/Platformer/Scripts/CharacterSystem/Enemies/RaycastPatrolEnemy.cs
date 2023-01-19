using Platformer.PlayerSystem;
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
        private Vector3 _horizontalSensorOrigin;
        [SerializeField]
        private float _horizontalSensorLength;
        [SerializeField]
        private float _verticalSensorLength;
        [SerializeField]
        private float _verticalSensorOffset;

        [SerializeField]
        private Vector3 _visiblityOrigin;
        [SerializeField]
        private float _frontVisibilityRange;
        [SerializeField]
        private float _behindVisibilityRange;

        private bool _canMove;
        private MovementDirection _direction;

        protected override void UpdateBehaviour()
        {
            // TODO: do all raycasts in fixed update
            if (!CheckCanMove() && !_pursuingPlayer)
            {
                StartCoroutine(StopAndWait(_idleTime));
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
            Ray horizontalCensor = GetHorizontalCensorRay(_horizontalSensorOrigin);
            Ray verticalCensor = GetVerticalCensorRay(_horizontalSensorOrigin);
            bool wallOnWay = Physics.Raycast(horizontalCensor, _horizontalSensorLength);
            bool HollowOnWay = Physics.Raycast(verticalCensor, _verticalSensorLength);
            return !wallOnWay && HollowOnWay;
        }

        private void CheckPlayerNearby()
        {
            Ray frontVisual = GetHorizontalCensorRay(_visiblityOrigin);
            Ray behindVisual = GetHorizontalCensorRay(_visiblityOrigin);

            Physics.Raycast(frontVisual, out RaycastHit frontHit, _frontVisibilityRange);
            Physics.Raycast(behindVisual.origin, -behindVisual.direction, out RaycastHit behindHit, _behindVisibilityRange);

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
            Vector3 playerPosition = _player.transform.position;
            Vector3 selfPosition = transform.position;
            if (playerPosition.x > selfPosition.x)
            {
                MovementController.MoveInput = 1f;
            }
            else if (playerPosition.x < selfPosition.x)
            {
                MovementController.MoveInput = -1f;
            }
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
            if (!_pursuingPlayer)
            {
                ChangePatrolDirection();
                Patrol();
            }
            _inIdle = false;
        }

        private Ray GetHorizontalCensorRay(Vector3 origin)
        {
            Vector3 startPoint = transform.TransformPoint(origin);
            Vector3 endPoint = transform.rotation * new Vector3(_horizontalSensorLength, 0, 0);
            return new Ray(startPoint, endPoint);
        }

        private Ray GetVerticalCensorRay(Vector3 origin)
        {
            origin.x = _verticalSensorOffset;
            Vector3 startPoint = transform.TransformPoint(origin);
            Vector3 endPoint = transform.rotation * new Vector3(0, -_verticalSensorLength, 0);
            return new Ray(startPoint, endPoint);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Ray horz = GetHorizontalCensorRay(_horizontalSensorOrigin);
            Gizmos.DrawRay(horz.origin, horz.direction * _horizontalSensorLength);
            Ray vert = GetVerticalCensorRay(_horizontalSensorOrigin);
            Gizmos.DrawRay(vert.origin, vert.direction * _verticalSensorLength);

            Gizmos.color = Color.magenta;

            Ray frontVisibility = GetHorizontalCensorRay(_visiblityOrigin);
            Gizmos.DrawRay(frontVisibility.origin, frontVisibility.direction * _frontVisibilityRange);

            Gizmos.color = Color.cyan;
            Ray behindVisibility = GetHorizontalCensorRay(_visiblityOrigin);
            Gizmos.DrawRay(behindVisibility.origin, behindVisibility.direction * -_behindVisibilityRange);
        }
#endif
    }
}