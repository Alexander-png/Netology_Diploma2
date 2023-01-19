using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
    public class RaycastPatrolEnemy : Enemy
    {
        [SerializeField]
        private Vector3 _horizontalRayOrigin;
        [SerializeField]
        private float _horizontalRayLength;
        [SerializeField]
        private float _verticalRayLength;
        [SerializeField]
        private float _verticalRayOffset;

        protected override void UpdateBehaviour()
        {
            if (CheckObstacles())
            {
                PatrolMove();
                //if (_attackingPlayer)
                //{
                //    PursuitPlayer();
                //}
                //else
                //{
                //    PatrolMove();
                //}
            }
        }

        private bool CheckObstacles()
        {
            Ray horizontalCensor = GetHorizontalRay();
            Ray verticalCensor = GetVerticalRay();
            bool isWallInWay = Physics.Raycast(horizontalCensor, _horizontalRayLength);
            bool isHollowOnWay = Physics.Raycast(verticalCensor, _verticalRayLength);
            return isWallInWay || isHollowOnWay;
        }

        private void PatrolMove()
        {            
            
        }

        private void PursuitPlayer()
        {

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