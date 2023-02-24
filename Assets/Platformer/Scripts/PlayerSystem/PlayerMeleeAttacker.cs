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
        private SphereCollider _sphereDamageTrigger;
        private float _defaultTriggerRadius;
        private float _currentTriggerRadius;
        private float _currentAttackRadius;

        private Coroutine _chargeAttackCoroutine;

        private PlayerInputListener _inputListener;

        protected override void Start()
        {
            base.Start();

            _playerMovement = transform.parent.GetComponent<CharacterMovement>();
            _inputListener = transform.parent.GetComponentInChildren<PlayerInputListener>();
            CurrentWeapon = transform.parent.GetComponentInChildren<MeleeWeapon>();

            _sphereDamageTrigger = _damageTrigger as SphereCollider;
            if (_sphereDamageTrigger == null)
            {
                GameLogger.AddMessage($"{nameof(PlayerMeleeAttacker)} now supports only {nameof(SphereCollider)} as damage trigger for resizing on strong attack. The resizing will not work for now.", GameLogger.LogType.Warning);
                return;
            }
            _defaultTriggerRadius = _sphereDamageTrigger.radius;
            ResetCurrentAttackStats();
        }

        protected override float GetDamageValue() =>
            _currentDamage + RawDamage;

        private void Update() =>
            UpdateHitColliderPosition();

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
            _chargeAttackCoroutine = StartCoroutine(ChargeAttack());
        }

        public override void OnAttackReleased()
        {
            if (_chargeAttackCoroutine != null)
            {
                StopCoroutine(_chargeAttackCoroutine);
                _chargeAttackCoroutine = null;
            }
            SetDamageTriggerRadius(_currentTriggerRadius);
            StartMainAttack();
        }

        protected override void OnHitEnded(object sender, EventArgs e)
        {
            CurrentWeapon?.StopHit();
            _damageTrigger.enabled = false;
            _playerMovement.MovementEnabled = true;
            ResetCurrentAttackStats();
        }

        private void UpdateHitColliderPosition()
        {
            Vector3 mousePosition = _inputListener.GetRelativeMousePosition(transform.parent.position);
            Ray ray = new Ray(transform.parent.position, mousePosition);
            transform.position = ray.GetPoint(_currentAttackRadius);
        }

        private void SetDamageTriggerRadius(float radius)
        {
            if (_sphereDamageTrigger != null)
            {
                _sphereDamageTrigger.radius = radius;
            }
        }

        private void ResetCurrentAttackStats()
        {
            _currentDamage = CurrentWeapon.Stats.Damage;
            _currentTriggerRadius = _defaultTriggerRadius;
            _currentAttackRadius = CurrentWeapon.Stats.AttackRadius;
            SetDamageTriggerRadius(_defaultTriggerRadius);
        }

        private IEnumerator ChargeAttack()
        {
            _playerMovement.MovementEnabled = false;
            float timeLeft = CurrentWeapon.Stats.StrongAttackChargeTime;

            float damageIncreaseStep = 
                _currentDamage * CurrentWeapon.Stats.StrongAttackDamageMultipler * 0.01f;

            float triggerRadiusIncreaseStep =
                _currentTriggerRadius * CurrentWeapon.Stats.StrongAttackRadiusMultipler * 0.01f;

            float attackRadiusIncreaseStep =
                _currentAttackRadius * CurrentWeapon.Stats.StrongAttackRadiusMultipler * 0.01f;

            while (timeLeft >= 0)
            {
                yield return null;
                timeLeft -= Time.deltaTime;
                _currentDamage += damageIncreaseStep;
                _currentTriggerRadius += triggerRadiusIncreaseStep;
                _currentAttackRadius += attackRadiusIncreaseStep;
            }
        }
    }
}