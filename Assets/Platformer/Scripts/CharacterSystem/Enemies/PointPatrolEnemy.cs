using Platformer.CharacterSystem.AI.Patroling;
using Platformer.CharacterSystem.Base;
using Platformer.Scriptable.Characters;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
	public class PointPatrolEnemy : MoveableEnemy
	{
        [SerializeField, Space(15)]
        protected Transform _patrolArea;
        protected PatrolPoint _currentPoint;

        protected override void Start()
        {
            FillPatrolPointList();
            base.Start();
        }

        protected virtual void FillPatrolPointList()
        {
            for (int i = 0; i < _patrolArea.childCount; i++)
            {
                if (_patrolArea.GetChild(i).TryGetComponent(out PatrolPoint point))
                {
                    _currentPoint = point;
                    break;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out MoveableEntity _))
            {
                Vector3 newVelocity = (-MovementController.Velocity + transform.up).normalized;
                newVelocity *= MovementController.MaxJumpForce;
                MovementController.Velocity = newVelocity;
            }
        }

        protected override void SetDefaultParameters(CharacterStats stats)
        {
            base.SetDefaultParameters(stats);
            _maxHealth = stats.MaxHealth;
            _currentHealth = _maxHealth;
        }

        protected override void UpdateBehaviour()
        {
            if (!_behaviourEnabled)
            {
                MovementController.MoveInput = 0f;
                _pursuingPlayer = false;
                return;
            }

            if (!_pursuingPlayer)
            {
                PatrolArea();
            }
            else
            {
                PursuitPlayer();
            }
        }

        private void PatrolArea()
        {
            if (_inIdle)
            {
                MovementController.MoveInput = 0f;
                MovementController.Velocity = Vector3.zero;
                return;
            }

            if (_currentPoint == null)
            {
                _currentPoint = _patrolArea.GetChild(0).GetComponent<PatrolPoint>();
            }

            var pointPos = _currentPoint.Position;

            MovementController.MoveInput = pointPos.x > transform.position.x ? 1f : -1f;

            if (Vector3.SqrMagnitude(transform.position - _currentPoint.Position) <= _currentPoint.ArriveRadius)
            {
                _currentPoint = _currentPoint.NextPoints[0];
                StartCoroutine(IdleCoroutine(_behaviourConfig.IdleTime));
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

            bool closeToPlayer = Mathf.Abs(playerPosition.x - selfPosition.x) <= _behaviourConfig.CloseToPlayerDistance;

            if (closeToPlayer)
            {
                bool needToJump = playerPosition.y - selfPosition.y >= _behaviourConfig.PlayerHeightDiffToJump;
                if (needToJump)
                {
                    MovementController.JumpInput = 1f;
                }
                else
                {
                    MovementController.JumpInput = 0f;
                }
            }
            else
            {
                MovementController.JumpInput = 0f;
            }
        }

        private IEnumerator IdleCoroutine(float idleTime)
        {
            _inIdle = true;
            yield return new WaitForSeconds(idleTime);
            _inIdle = false;
        }
    }
}