using Platformer.CharacterSystem.Base;
using Platformer.Scriptable.Characters;
using Platformer.SkillSystem;
using System;
using System.Collections;
using UnityEngine;

namespace Platformer.PlayerSystem
{
    public class Player : MoveableEntity, IDamagable, ISkillObservable
    {
        private Inventory _inventory;     
        private SkillObserver _skillObserver;     
        private PlayerInputListener _playerInputListener;

        private bool _damageImmune = false;
        private float _damageImmuneTime;
        private float _currentHealth;
        private float _maxHealth;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;

        public Inventory Inventory => _inventory;
        public SkillObserver SkillObserver => _skillObserver;

        public event EventHandler Died;

        protected override void Start()
        {
            base.Start();
            _inventory = gameObject.GetComponent<Inventory>();
            _skillObserver = gameObject.GetComponent<SkillObserver>();
            _playerInputListener = gameObject.GetComponent<PlayerInputListener>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _damageImmune = false;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopAllCoroutines();
        }

        protected override void SetDefaultParameters(CharacterStats stats)
        {
            base.SetDefaultParameters(stats);
            _maxHealth = stats.MaxHealth;
            _currentHealth = _maxHealth;
            _damageImmuneTime = stats.DamageImmuneTime;
        }

        public void SetDamage(float damage, Vector3 pushVector, bool forced = false)
        {
            if (_damageImmune && !forced || _currentHealth <= 0f)
            {
                return;
            }

            StopCoroutine(DamageImmuneCoroutine(_damageImmuneTime));
            MovementController.Velocity = pushVector;
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
            if (_currentHealth > 0)
            {
                StartCoroutine(DamageImmuneCoroutine(_damageImmuneTime));
            }

            if (_currentHealth < 0.01f)
            {
                Died?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Heal(float value) =>
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _maxHealth);

        protected override void UpdateRotation()
        {
            Vector3 relativeMousePos = _playerInputListener.GetMousePositionInWorld() - transform.position;
            float rotation = relativeMousePos.x > 0 ? 0 : 180;
            transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        }

        private IEnumerator DamageImmuneCoroutine(float time)
        {
            _damageImmune = true;
            yield return new WaitForSeconds(time);
            _damageImmune = false;
        }
    }
}