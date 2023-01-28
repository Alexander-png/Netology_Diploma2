using System;
using UnityEngine;

namespace Platformer.Weapons
{
	public class PlayerWeapon : MeleeWeapon
	{
        [SerializeField]
        private Transform _visual;
        [SerializeField]
        private Animator _animator;

        private void OnEnable() =>
            _owner.Respawning += OnPlayerRespawning;

        private void OnDisable() =>
            _owner.Respawning -= OnPlayerRespawning;

        private void Start() => ResetValues();

        public override void MakeHit()
        {
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
            InvokeHitEnded();
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