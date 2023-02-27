using Platformer.CharacterSystem.Base;
using Platformer.CharacterSystem.Enemies;
using Platformer.Scriptable.Skills.Data;
using Platformer.Weapons;
using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public class MeleeAttacker : Attacker
	{
        [SerializeField]
        private CombatSkillConfiguration _defaultSkills;

        private CombatSkillData _combatSkillData;

        public float RawDamage => _combatSkillData.Damage;
        public float RawReloadTime => _combatSkillData.ReloadTime;

        private MeleeWeapon _currentWeapon;
        protected Collider _damageTrigger;

        protected MeleeWeapon CurrentWeapon
        {  
            get => _currentWeapon;
            set
            {
                if (_currentWeapon != null)
                {
                    _currentWeapon.HitEnded -= OnHitEnded;
                }
                _currentWeapon = value;
                if (_currentWeapon != null)
                {
                    _currentWeapon.HitEnded += OnHitEnded;
                }
            }
        }

        protected virtual void Start()
        {
            CurrentWeapon = GetComponentInChildren<MeleeWeapon>();
            _damageTrigger = GetComponent<Collider>();
            _damageTrigger.enabled = false;
            _combatSkillData = _defaultSkills.GetData();
        }

        private void OnDisable() =>
            CurrentWeapon = null;

        private void OnDestroy() =>
            CurrentWeapon = null;

        public override void OnMainAttackPressed() =>
            StartMainAttack();

        public override void OnSecondAttackPressed() =>
            CurrentWeapon.MakeAlternativeHit();

        public override void OnStrongAttackPressed() { }

        protected virtual void StartMainAttack()
        {
            _currentWeapon?.MakeHit();
            _damageTrigger.enabled = true;
        }

        public override void OnAttackReleased()
        {
            _currentWeapon?.StopHit();
            _damageTrigger.enabled = false;
        }

        protected virtual void OnHitEnded(object sender, System.EventArgs e) =>
            OnAttackReleased();

        protected virtual IDamagable GetEnemyComponent(Collider other)
        {
            // My game architechture went bad and this showed here.
            // TODO: resolve this problem.
            other.TryGetComponent(out Entity entity);
            if (entity != null)
            {
                if (entity is MoveableEnemy || entity is StationaryEnemy)
                { 
                    return entity as IDamagable; 
                }
            }
            return null;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            var enemy = GetEnemyComponent(other);
            if (enemy != null)
            {
                if (_currentWeapon != null)
                {
                    enemy.SetDamage(GetDamageValue(), (other.transform.position - transform.position) * CurrentWeapon.Stats.PushForce);
                }
            }
        }

        protected virtual float GetDamageValue() =>
            CurrentWeapon.Stats.Damage + RawDamage;

        public override float GetAttackChargeTime() =>
            CurrentWeapon.Stats.StrongAttackChargeTime;

        public override void AddSkill(CombatSkillData toAdd)
        {
            base.AddSkill(toAdd);
            CurrentWeapon.OnSkillAddedToAttacker(toAdd);
            _combatSkillData += toAdd;
        }

        public override void RemoveSkill(CombatSkillData toRemove)
        {
            base.RemoveSkill(toRemove);
            CurrentWeapon.OnSkillRemovedFromAttacker(toRemove);
            _combatSkillData -= toRemove;
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            // TODO: somehow draw rotated cube. It is pssible with gizmos.matrix.
            Color c = Color.red;
            c.a = 0.3f;
            Gizmos.color = c;
            if (TryGetComponent(out BoxCollider box))
            {
                Gizmos.DrawCube(box.bounds.center, box.bounds.size);
            }
            else if (TryGetComponent(out SphereCollider sphere))
            {
                Gizmos.DrawSphere(transform.position, sphere.radius);
            }
        }
#endif
    }
}