using System;
using UnityEngine;

namespace Platformer.Weapons
{
	public class PlayerWeapon : BasicWeapon
	{
        [SerializeField]
        private Transform _visual;
        [SerializeField]
        private Animator _animator;

        public event EventHandler HitEnded;

        private void OnEnable() =>
            _owner.Respawning += OnPlayerRespawning;

        private void OnDisable() =>
            _owner.Respawning -= OnPlayerRespawning;

        private void Start() => ResetValues();

        public void MakeHit()
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
            HitEnded?.Invoke(this, EventArgs.Empty);
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