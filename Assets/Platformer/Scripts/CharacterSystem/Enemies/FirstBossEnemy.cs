using Platformer.CharacterSystem.Base;
using Platformer.EditorExtentions;
using Platformer.GameCore;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
    public class FirstBossEnemy : PatrolEnemy
    {
        [SerializeField]
        private TriggerEnterNotifier _fightTrigger;

        private bool _reloadingDash = false;
        private bool _chargingDash = false;
        private bool _dashCooldown = false;
        private int _dashesLeft;
        private Coroutine _dashChargeCoroutine;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (_fightTrigger == null)
            {
                GameLogger.AddMessage($"No fight trigger specified for {gameObject.name}. The battle will not start.", GameLogger.LogType.Error);
                return;
            }
            _fightTrigger.OnPlayerEnteredTrigger += OnBossFightBegin;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (_fightTrigger == null)
            {
                return;
            }
            _fightTrigger.OnPlayerEnteredTrigger -= OnBossFightBegin;
        }

        private void OnBossFightBegin(object sender, System.EventArgs e) =>
            OnPlayerNearby();

        protected override void Start()
        {
            base.Start();
            StartCoroutine(ReloadDashUsage());
        }

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

        public override void OnPlayerRanAway()
        {
            base.OnPlayerRanAway();

            if (_dashChargeCoroutine != null)
            {
                StopCoroutine(_dashChargeCoroutine);
                _dashChargeCoroutine = null;
            }
            _chargingDash = false;
            StartCoroutine(DashCooldown());
        }

        protected override void Patrol()
        {
            if (_chargingDash)
            {
                return;
            }
            base.Patrol();
        }

        private void PursuitPlayer()
        {
            if (_chargingDash)
            {
                return;
            }
            _attacker.OnSecondAttackPressed();

            if (!CloseToPlayer)
            {
                MovementController.HorizontalInput = CalculateHorizontalInput();
            }
            BeginDash();
        }

        private float CalculateHorizontalInput()
        {
            float result = 0f;
            Vector3 playerPosition = _player.transform.position;
            Vector3 selfPosition = transform.position;
            if (playerPosition.x > selfPosition.x)
            {
                result = 1f;
            }
            else if (playerPosition.x < selfPosition.x)
            {
                result = -1f;
            }
            return result;
        }

        private void BeginDash() =>
            _dashChargeCoroutine = StartCoroutine(ChargeAndDash());

        private bool CanUseDash() => 
            !_reloadingDash && 
            !MovementController.IsDashing && 
            !_chargingDash &&
            !_dashCooldown;

        private IEnumerator ChargeAndDash()
        {
            if (!CanUseDash() || _dashChargeCoroutine != null)
            {
                yield break;
            }

            MovementController.UpdateRotation();
            MovementController.StopImmediatly();
            InvokeEntityEvent(EntityEventTypes.Idle);
            _chargingDash = true;
            yield return new WaitForSeconds(_behaviourConfig.DashChargeTime);
            MovementController.HorizontalInput = CalculateHorizontalInput();
            _chargingDash = false;
            _dashesLeft -= 1;
            InvokeEntityEvent(EntityEventTypes.DashStarted);
            MovementController.TriggerDash(1);

            StartCoroutine(DashCooldown());
            if (_dashesLeft == 0)
            {
                StartCoroutine(ReloadDashUsage());
            }
        }

        private IEnumerator DashCooldown()
        {
            if (_dashCooldown)
            {
                yield break;
            }
            _dashCooldown = true;
            yield return new WaitForSeconds(_behaviourConfig.DashCooldownTime);
            _dashCooldown = false;
        }

        private IEnumerator ReloadDashUsage()
        {
            if (_reloadingDash)
            {
                yield break;
            }
            _reloadingDash = true;
            yield return new WaitForSeconds(_behaviourConfig.DashUsageReloadTime);
            _reloadingDash = false;
            _dashesLeft = _behaviourConfig.DashCountWithoutReload;
        }

#if UNITY_EDITOR
        [ContextMenu("Invoke dash")]
        private void Dash()
        {
            BeginDash();
        }
#endif
    }
}