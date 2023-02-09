using Platformer.Scriptable.Characters.AIConfig;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
    public class FirstBossEnemy : PatrolEnemy
    {
        [SerializeField]
        private ViewFieldConfig _viewFieldConfig;

        private bool _reloadingDash = false;
        private bool _chargingDash = false;
        private bool _dashCooldown = false;
        private int _dashesLeft;
        private Coroutine _dashChargeCoroutine;

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

        protected override void FixedUpdateBehaviour() =>
            CheckPlayerNearby();

        protected override void CheckPlayerNearby()
        {
            Ray visualRay = GetHorizontalCensorRay(_viewFieldConfig.ViewOrigin);

            Physics.Raycast(visualRay, out RaycastHit frontHit, _viewFieldConfig.FrontViewRange, PlayerLayer);
            Physics.Raycast(visualRay.origin, -visualRay.direction, out RaycastHit behindHit, _viewFieldConfig.BehindViewRange, PlayerLayer);

            bool seePlayer = frontHit.transform != null || behindHit.transform != null;

            if (seePlayer)
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
            base.OnPlayerNearby();
            _attacker.OnSecondAttackPressed();
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

        private void PursuitPlayer()
        {
            if (_chargingDash)
            {
                return;
            }
            MovementController.HorizontalInput = CalculateHorizontalInput();
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

            UpdateRotation();
            MovementController.StopImmediatly();
            _chargingDash = true;
            yield return new WaitForSeconds(_behaviourConfig.DashChargeTime);
            MovementController.HorizontalInput = CalculateHorizontalInput();
            _chargingDash = false;
            _dashesLeft -= 1;
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
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (_viewFieldConfig != null)
            {
                Gizmos.color = Color.magenta;
                Ray frontVisibility = GetHorizontalCensorRay(_viewFieldConfig.ViewOrigin);
                Gizmos.DrawRay(frontVisibility.origin, frontVisibility.direction * _viewFieldConfig.FrontViewRange);

                Gizmos.color = Color.cyan;
                Ray behindVisibility = GetHorizontalCensorRay(_viewFieldConfig.ViewOrigin);
                Gizmos.DrawRay(behindVisibility.origin, behindVisibility.direction * -_viewFieldConfig.BehindViewRange);
            }
            else
            {
                EditorExtentions.GameLogger.AddMessage("Please set view field config", EditorExtentions.GameLogger.LogType.Warning);
            }
        }
#endif
    }
}