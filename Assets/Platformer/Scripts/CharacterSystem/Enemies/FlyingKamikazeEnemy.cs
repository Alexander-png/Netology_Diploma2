using Platformer.CharacterSystem.AI.Patroling;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
	public class FlyingKamikazeEnemy : MoveableEnemy
    {
        [SerializeField]
        protected PatrolPoint _currentPoint;

        private Coroutine _waitCoroutine;
        private bool _chargingAttack;
        private bool _attacking;
        private Coroutine _chargeAttackCoroutine;

        // todo: check is player in directly visible

        protected override void FixedUpdateBehaviour()
        {
            Patrol();

            //CheckPlayerNearby();
            //if (_pursuingPlayer)
            //{
            //    AimPlayer();
            //}
            //else
            //{
            //    Patrol();
            //}
        }

        private void AimPlayer()
        {
            if (_chargeAttackCoroutine == null)
            {
                _chargeAttackCoroutine = StartCoroutine(ChargeAttack());
            }

            Vector3 selfPosition = transform.position;
            Vector3 playerPosition = _player.transform.position;
            



            //float angle = Vector3.Angle(selfPosition, playerPosition);
            //angle -= 90;
            //Vector3 cross = Vector3.Cross(selfPosition, playerPosition);
            //if (cross.z < 0)
            //{
            //    angle = -angle;
            //}

            //Vector3 resultRotation = new Vector3(0, 0, angle);
            //if (playerPosition.x < selfPosition.x)
            //{
            //    resultRotation.y = 180;
            //}
            //else
            //{
            //    resultRotation.y = 0;
            //}

            //Debug.Log(angle);

            //transform.rotation = Quaternion.Euler(resultRotation);

            
            // todo: aiming to player 

        }

        private void Patrol()
        {
            if (_inIdle)
            {
                MovementController.HorizontalInput = 0f;
                MovementController.Velocity = Vector3.zero;
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
            // todo: dash to player
        }

        protected override void CheckPlayerNearby()
        {
            Vector3 playerPosition = _player.transform.position;
            float distanceToPlayer = (playerPosition - transform.position).sqrMagnitude;

            if (distanceToPlayer <= _behaviourConfig.ArgressionRadius)
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

            base.OnPlayerNearby();
            if (_waitCoroutine != null)
            {
                StopCoroutine(_waitCoroutine);
            }
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