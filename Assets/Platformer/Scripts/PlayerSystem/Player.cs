using Platformer.CharacterSystem.Base;
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
        private Coroutine _damageImmuneCoroutine;

        private bool _damageImmune = false;
        private float _currentHealth;

        public float MaxHealth => _currentStats.MaxHealth;
        public float DamageImmuneTime => _currentStats.DamageImmuneTime;
        public float CurrentHealth => _currentHealth;
        public bool CanBeDamaged => _damageImmuneCoroutine == null;

        public Inventory Inventory => _inventory;
        public SkillObserver SkillObserver => _skillObserver;

        public event EventHandler Died;

        protected override void Start()
        {
            base.Start();
            _currentHealth = MaxHealth;
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

        public void SetDamage(float damage, Vector3 pushVector, bool forced = false)
        {
            if (_damageImmune && !forced || _currentHealth <= 0f)
            {
                return;
            }

            MovementController.Velocity = pushVector;
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, MaxHealth);
            if (_currentHealth > 0)
            {
                InvokeEntityEvent(EntityEventTypes.Damage);
                _damageImmuneCoroutine = StartCoroutine(DamageImmuneCoroutine(DamageImmuneTime));
            }

            if (_currentHealth < 0.01f)
            {
                InvokeEntityEvent(EntityEventTypes.Death);
                gameObject.SetActive(false);
                Died?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Heal(float value)
        {
            InvokeEntityEvent(EntityEventTypes.Heal);
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0, MaxHealth);
        }

        protected override void UpdateRotation()
        {
            Vector3 relativeMousePos = _playerInputListener.GetRelativeMousePosition(transform.position);
            float rotation = relativeMousePos.x > 0 ? 0 : 180;
            transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        }

        private IEnumerator DamageImmuneCoroutine(float time)
        {
            _damageImmune = true;
            yield return new WaitForSeconds(time);
            _damageImmune = false;
            _damageImmuneCoroutine = null;
        }
    }
}