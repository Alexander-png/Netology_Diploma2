using Platformer.CharacterSystem.Base;
using Platformer.Scriptable.EntityConfig;
using System;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
    public abstract class StationaryEnemy : Entity, IDamagable
    {
        protected const int PlayerLayer = 1 << 6;

        protected float _currentHealth;
        protected float _maxHealth;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public event EventHandler Died;

        protected override void SetDefaultParameters(CharacterStats stats)
        {
            base.SetDefaultParameters(stats);
            _maxHealth = stats.MaxHealth;
            _currentHealth = _maxHealth;
        }

        public void Heal(float value) => 
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _maxHealth);

        public void SetDamage(float damage, Vector3 pushVector, bool forced = false)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
            if (_currentHealth < 0.01f)
            {
                EditorExtentions.GameLogger.AddMessage($"Enemy with name {gameObject.name} was. Killed. You can implement respawn system");
                gameObject.SetActive(false);
            }
        }
    }
}