using Platformer.CharacterSystem.Base;
using System;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
    public abstract class StationaryEnemy : Entity, IDamagable
    {
        protected const int PlayerLayer = 1 << 6;
        protected float _currentHealth;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _currentStats.MaxHealth;
        public bool CanBeDamaged => true;

        public event EventHandler Died;

        protected override void Start()
        {
            base.Start();
            _currentHealth = MaxHealth;
        }

        public void Heal(float value) => 
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0, MaxHealth);

        public void SetDamage(float damage, Vector3 pushVector, bool forced = false)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, MaxHealth);
            if (_currentHealth < 0.01f)
            {
                EditorExtentions.GameLogger.AddMessage($"Enemy with name {gameObject.name} was. Killed. You can implement respawn system");
                gameObject.SetActive(false);
            }
        }
    }
}