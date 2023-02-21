using System;
using UnityEngine;

namespace Platformer.CharacterSystem.AI.Patroling
{
	public class PatrolPoint : MonoBehaviour
	{
        [SerializeField]
        private PatrolPoint[] _nextPoints;
        [SerializeField]
        private float _arriveRadius;

        public Vector3 Position => transform.position;

        public PatrolPoint[] NextPoints
        {
            get
            {
                if (_nextPoints == null)
                {
                    return null;
                }
                PatrolPoint[] toReturn = new PatrolPoint[_nextPoints.Length];
                Array.Copy(_nextPoints, toReturn, _nextPoints.Length);
                return toReturn;
            }
        }

        public float ArriveRadius => _arriveRadius;
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color pointColor = Color.blue;
            pointColor.a = 0.5f;
            Gizmos.color = pointColor;
            Gizmos.DrawSphere(transform.position, ArriveRadius);

            Gizmos.color = Color.green;

            foreach (var point in _nextPoints)
            {
                Gizmos.DrawLine(transform.position, point.Position);
            }
        }
#endif
    }
}