using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
	public abstract class PatrolEnemy : MoveableEnemy
	{
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
    }
}