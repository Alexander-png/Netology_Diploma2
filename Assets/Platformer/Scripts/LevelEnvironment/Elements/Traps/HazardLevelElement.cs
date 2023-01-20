using Platformer.CharacterSystem.Base;
using Platformer.LevelEnvironment.Elements.Common;
using Platformer.Scriptable.LevelElements.Traps;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Interactables.Elements.Traps
{
	public class HazardLevelElement : Platform
	{
		[SerializeField]
		protected DamageStats _stats;
        [SerializeField]
        protected bool _trapEnabled = true;

        public bool TrapEnabled
        {
            get => _trapEnabled; 
            set
            {
                _trapEnabled = value;
                if (!_trapEnabled)
                {
                    ResetState();
                }
            }
        }

        // TODO: is bad solution or not?
        public bool DamageEnabled { get; protected set; } = true;

        protected List<IDamagable> _touchingCharacters = new List<IDamagable>();

        private void OnEnable()
        {
            GameSystem.PlayerRespawned += OnPlayerRespawned;
        }

        private void OnDisable()
        {
            GameSystem.PlayerRespawned -= OnPlayerRespawned;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_trapEnabled || !DamageEnabled)
            {
                return;
            }

            if (collision.gameObject.TryGetComponent(out IDamagable character))
            {
                _touchingCharacters.Add(character);
            }
        }

        private void FixedUpdate()
        {
            if (!_trapEnabled || !DamageEnabled)
            {
                return;
            }

            for (int i = 0; i < _touchingCharacters.Count; i++)
            {
                if (_touchingCharacters[i] != null)
                {
                    _touchingCharacters[i].SetDamage(_stats.Damage, transform.up * _stats.PushForce);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!_trapEnabled || !DamageEnabled)
            {
                return;
            }

            if (collision.gameObject.TryGetComponent(out IDamagable character))
            {
                _touchingCharacters.Remove(character);
            }
        }

        private void OnPlayerRespawned(object sender, System.EventArgs e) => ResetState(true);
        protected virtual void ResetState(bool playerDied = false) 
        { 
            _touchingCharacters.Clear(); 
            DamageEnabled = true;
        }
    }
}