using Platformer.CharacterSystem.AI.Patroling;
using Platformer.PlayerSystem;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
    // todo: inherit this class from patrol enemy when point patrol mode will be implemented
	public class FlyingKamikazeEnemy : MoveableEnemy
    {
        [SerializeField]
        protected PatrolPoint _currentPoint;

        //private Coroutine _waitCoroutine;
        private bool _chargingAttack;
        private bool _attacking;
        private Coroutine _chargeAttackCoroutine;

        protected override void FixedUpdateBehaviour()
        {
            if (_attacking)
            {
                return;
            }

            CheckPlayerNearby();
            if (_pursuingPlayer)
            {
                AimPlayer();
            }
            else
            {
                Patrol();
            }
        }

        private void AimPlayer()
        {
            if (_chargeAttackCoroutine == null)
            {
                _chargeAttackCoroutine = StartCoroutine(ChargeAttack());
            }

            Vector3 selfPos = transform.position;
            Vector3 playerPos = _player.transform.position;
            float targetAngle = Mathf.Atan2(playerPos.y - selfPos.y, playerPos.x - selfPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
        }

        private void Patrol()
        {
            if (_inIdle)
            {
                MovementController.StopImmediatly();
                return;
            }

            var pointPos = _currentPoint.Position;

            MovementController.HorizontalInput = pointPos.x > transform.position.x ? 1f : -1f;
            if (pointPos.y > transform.position.y)
            {
                MovementController.VerticalInput = 1f;
            }
            else if (pointPos.y < transform.position.y)
            {
                MovementController.VerticalInput = -1f;
            }
            else
            {
                MovementController.VerticalInput = 0;
            }

            if (Vector3.SqrMagnitude(transform.position - _currentPoint.Position) <= _currentPoint.ArriveRadius)
            {
                _currentPoint = _currentPoint.NextPoints[0];
                StartCoroutine(IdleCoroutine(_behaviourConfig.IdleTime));
            }
        }

        private void AttackPlayer()
        {
            _attacking = true;

            Vector3 pos = transform.position;
            Vector3 dir = (_player.transform.position - pos).normalized;

            MovementController.HorizontalInput = dir.x;
            MovementController.VerticalInput = dir.y;
            MovementController.DashInput = 1f;
        }

        protected override void CheckPlayerNearby()
        {
            Vector3 playerPosition = _player.transform.position;
            float distanceToPlayer = (playerPosition - transform.position).sqrMagnitude;

            if (distanceToPlayer <= _behaviourConfig.ArgressionRadius && CheckSeePlayer())
            {
                OnPlayerNearby();
            }
            else
            {
                OnPlayerRanAway();
            }
        }

        public override void OnPlayerNearby()
        {
            if (_pursuingPlayer)
            {
                return;
            }

            MovementController.StopImmediatly();

            base.OnPlayerNearby();
            //if (_waitCoroutine != null)
            //{
            //    StopCoroutine(_waitCoroutine);
            //}
        }

        private bool CheckSeePlayer()
        {
            Vector3 pos = transform.position;
            Vector3 dir = (_player.transform.position - pos).normalized;
            Ray ray = new Ray(transform.position, dir);
            if (Physics.Raycast(ray, out RaycastHit hit, _behaviourConfig.ArgressionRadius))
            {
                if (hit.transform.gameObject.TryGetComponent(out Player _))
                {
                    return true;
                }
            }
            return false;
        }

        public override void OnPlayerRanAway()
        {
            if (!_pursuingPlayer)
            {
                return;
            }
            base.OnPlayerRanAway();
            if (_chargeAttackCoroutine != null)
            {
                StopCoroutine(_chargeAttackCoroutine);
            }
            _chargeAttackCoroutine = null;
            _chargingAttack = false;
        }

        protected IEnumerator ChargeAttack()
        {
            if (_chargingAttack)
            {
                yield break;
            }
            
            _chargingAttack = true;
            yield return new WaitForSeconds(_attacker.GetAttackChargeTime());
            AttackPlayer();
            _chargeAttackCoroutine = null;
        }

        private IEnumerator IdleCoroutine(float idleTime)
        {
            _inIdle = true;
            yield return new WaitForSeconds(idleTime);
            _inIdle = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color c = Color.cyan;
            c.a = 0.6f;
            Gizmos.color = c;
            Gizmos.DrawSphere(transform.position, Mathf.Sqrt(_behaviourConfig.ArgressionRadius));
        }
#endif
    }
}