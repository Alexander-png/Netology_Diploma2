using Platformer.CharacterSystem.Attacking;
using Platformer.CharacterSystem.Movement.Base;
using Platformer.EditorExtentions;
using Platformer.Weapons;
using System;
using System.Collections;
using UnityEngine;

namespace Platformer.PlayerSystem
{
	public class PlayerMeleeAttacker : MeleeAttacker
	{
        private CharacterMovement _playerMovement;

        private float _currentDamage;
        private BoxCollider _boxDamageTrigger;
        private float _defaultTriggerRadiusZ;
        private float _damageTriggerRadiusZ;
        private float _currentAttackRadius;

        private Coroutine _chargeAttackCoroutine;

        private PlayerInputListener _inputListener;

        protected override void Start()
        {
            base.Start();

            _playerMovement = transform.parent.GetComponent<CharacterMovement>();
            _inputListener = transform.parent.GetComponentInChildren<PlayerInputListener>();
            CurrentWeapon = transform.parent.GetComponentInChildren<MeleeWeapon>();

            _boxDamageTrigger = _damageTrigger as BoxCollider;
            if (_boxDamageTrigger == null)
            {
                GameLogger.AddMessage($"{nameof(PlayerMeleeAttacker)} now supports only {nameof(SphereCollider)} as damage trigger for resizing on strong attack. The resizing will not work for now.", GameLogger.LogType.Warning);
                return;
            }
            _defaultTriggerRadiusZ = CurrentWeapon.Stats.AttackRadius;
            SetDamageTriggerRadius(_defaultTriggerRadiusZ);
            ResetCurrentAttackStats();
        }

        protected override float GetDamageValue() =>
            _currentDamage + RawDamage;

        private void Update() =>
            UpdateDamageColliderState();

        public override void OnMainAttackPressed()
        {
            if (CurrentWeapon?.CanNotAttack() == true)
            {
                return;
            }
            ResetCurrentAttackStats();
        }

        public override void OnStrongAttackPressed()
        {
            if (_chargeAttackCoroutine != null)
            {
                return;
            }
            _chargeAttackCoroutine = StartCoroutine(ChargeAttack());
        }

        public override void OnAttackReleased()
        {
            if (CurrentWeapon?.CanNotAttack() == true)
            {
                return;
            }
            if (_chargeAttackCoroutine != null)
            {
                StopCoroutine(_chargeAttackCoroutine);
                _chargeAttackCoroutine = null;
                _currentAttackRadius = (CurrentWeapon.Stats.AttackRadius + _damageTriggerRadiusZ) / 2;
            }
            SetDamageTriggerRadius(_damageTriggerRadiusZ);
            StartMainAttack();
        }

        protected override void OnHitEnded(object sender, EventArgs e)
        {
            CurrentWeapon?.StopHit();
            _damageTrigger.enabled = false;
            _playerMovement.MovementEnabled = true;
            ResetCurrentAttackStats();
        }

        private void UpdateDamageColliderState()
        {
            Vector3 mousePosition = _inputListener.GetRelativeMousePosition(transform.parent.position);
            Ray ray = new Ray(transform.parent.position, mousePosition);
            transform.position = ray.GetPoint(_currentAttackRadius);
            Vector3 absoluteMousePos = _inputListener.GetWorldMousePosition();
            transform.LookAt(absoluteMousePos);
        }

        private void SetDamageTriggerRadius(float radius)
        {
            if (_boxDamageTrigger != null)
            {
                Vector3 triggerSize = _boxDamageTrigger.size;
                triggerSize.z = radius;
                _boxDamageTrigger.size = triggerSize;
            }
        }

        private void ResetCurrentAttackStats()
        {
            _currentDamage = CurrentWeapon.Stats.Damage;
            _damageTriggerRadiusZ = _defaultTriggerRadiusZ;
            _currentAttackRadius = CurrentWeapon.Stats.AttackRadius;
            SetDamageTriggerRadius(_defaultTriggerRadiusZ);
        }

        private IEnumerator ChargeAttack()
        {
            _playerMovement.MovementEnabled = false;
            float timeLeft = CurrentWeapon.Stats.StrongAttackChargeTime;

            float damageIncreaseStep = 
                _currentDamage * CurrentWeapon.Stats.StrongAttackDamageMultipler * 0.01f;

            float triggerRadiusIncreaseStep =
                _damageTriggerRadiusZ * CurrentWeapon.Stats.StrongAttackRadiusMultipler * 0.01f;

            float attackRadiusIncreaseStep =
                _currentAttackRadius * CurrentWeapon.Stats.StrongAttackRadiusMultipler * 0.01f;

            while (timeLeft >= 0)
            {
                yield return null;
                timeLeft -= Time.deltaTime;

                _currentDamage += damageIncreaseStep;
                _damageTriggerRadiusZ += triggerRadiusIncreaseStep;
                _currentAttackRadius += attackRadiusIncreaseStep;
            }
        }
    }
}