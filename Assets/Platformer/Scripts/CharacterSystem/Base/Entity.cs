using Platformer.CharacterSystem.StatsData;
using Platformer.EditorExtentions;
using Platformer.GameCore.Helpers;
using Platformer.Scriptable.EntityConfig;
using System;
using UnityEngine;

namespace Platformer.CharacterSystem.Base
{
    public enum EnitityEventTypes : byte
    { 
        Heal,
        Damage,
        Death,
        Respawn,
        DashStarted,
        DashEnded,
        Landing,
    }

    public abstract class Entity : MonoBehaviour
    {
        [SerializeField]
        private CharacterStats _stats;

        private Animator _entityAnimator;

        public event EventHandler Respawning;
        public event EventHandler<EnitityEventTypes> EventInvoked;

        public string Name { get; protected set; }
        public Animator EntityAnimator => _entityAnimator;

        protected virtual void Awake() 
        {
            if (_stats == null)
            {
                GameLogger.AddMessage($"{nameof(Entity)} ({gameObject.name}): no stats assigned.", GameLogger.LogType.Fatal);
            }
            SetDefaultParameters(_stats);
        }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void Start()
        {
            Visual visual = GetComponentInChildren<Visual>();
            if (visual != null)
            {
                _entityAnimator = visual.transform.GetComponent<Animator>();
            }
        }
        protected virtual void Update() { }
        protected virtual void FixedUpdate() { }

        protected virtual void SetDefaultParameters(CharacterStats stats) =>
            Name = stats.Name;

        public virtual void AddStats(CharacterStatsData stats) { }

        public virtual void RemoveStats(MovementStatsData stats) { }

        public virtual void NotifyRespawn() => 
            Respawning?.Invoke(this, EventArgs.Empty);

        public void InvokeEntityEvent(EnitityEventTypes e) =>
            EventInvoked?.Invoke(this, e);

        public void SetAnimatorState(string name, float value)
        {
            if (_entityAnimator != null)
            {
                _entityAnimator.SetFloat(name, value);
            }
        }
    }
}
