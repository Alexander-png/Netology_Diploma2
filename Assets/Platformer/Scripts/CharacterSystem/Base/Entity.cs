using Platformer.EditorExtentions;
using Platformer.Scriptable.Skills.Data;
using System;
using UnityEngine;

namespace Platformer.CharacterSystem.Base
{
    public enum EntityEventTypes : byte
    { 
        Heal,
        Damage,
        Death,
        Respawn,
        DashStarted,
        DashEnded,
        Landing,
        Idle,
        IdleLong,
        Walk,
        Attack,
        ResetState,
    }

    public abstract class Entity : MonoBehaviour
    {
        [SerializeField]
        private StatsSkillConfiguration _baseStats;

        protected CharacterSkillData _currentStats;

        public event EventHandler Respawning;
        public event EventHandler<EntityEventTypes> EventInvoked;

        public string Name { get; protected set; }

        protected virtual void Awake() 
        {
            if (_baseStats == null)
            {
                GameLogger.AddMessage($"{nameof(Entity)} ({gameObject.name}): no stats assigned.", GameLogger.LogType.Fatal);
            }
            _currentStats = _baseStats.GetData();
        }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void Start() { }

        protected virtual void Update() { }
        protected virtual void FixedUpdate() { }

        public virtual void AddSkill(CharacterSkillData stats) =>
            _currentStats += stats;

        public virtual void RemoveSkill(CharacterSkillData stats) =>
            _currentStats -= stats;

        public virtual void NotifyRespawn() => 
            Respawning?.Invoke(this, EventArgs.Empty);

        public virtual void InvokeEntityEvent(EntityEventTypes e)
        {
            if (EventInvoked != null)
            {
                EventInvoked.Invoke(this, e);
            }
            else
            {
                OnEventProcessed(e);
            }
        }

        public virtual void OnEventProcessed(EntityEventTypes e) { }
    }
}
