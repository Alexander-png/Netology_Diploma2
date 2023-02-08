using Platformer.GameCore.Helpers;
using System;
using UnityEngine;

namespace Platformer.Weapons
{
	public class PlayerWeapon : MeleeWeapon
	{
        private Transform _visual;
        private Animator _animator;

        protected override void OnEnable()
        {
            base.OnEnable();
            _owner.Respawning += OnPlayerRespawning;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _owner.Respawning -= OnPlayerRespawning;
        }

        private void Awake()
        {
            FindOwner();
        }

        protected override void Start()
        {
            base.Start();
            _visual = GetComponentInChildren<Visual>().transform;
            _animator = GetComponent<Animator>();
            ResetValues();
        }

        public override void MakeHit()
        {
            if (CanNotAttack())
            {
                return;
            }
            _animator.SetFloat("Hit", 1);
            _visual.gameObject.SetActive(true);
        }

        private void ResetValues()
        {
            _animator.SetFloat("Hit", 0);
            _visual.gameObject.SetActive(false);
        }

        public void OnHitEnd()
        {
            _attacking = false;
            InvokeHitEnded();
            StartCoroutine(ReloadMainAttack());
            ResetValues();
        }

        private void OnPlayerRespawning(object sender, EventArgs e)
        {
            ResetAnimator();
            ResetValues();
        }

        private void ResetAnimator()
        {
            _animator.enabled = false;
            _animator.enabled = true;
        }
    }
}