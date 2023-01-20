using Platformer.EditorExtentions;
using Platformer.Scriptable.Characters;
using System;
using UnityEngine;

namespace Platformer.CharacterSystem.Base
{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField]
        private CharacterStats _stats;

        public event EventHandler Respawning;

        public string Name { get; protected set; }

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
        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void FixedUpdate() { }

        protected virtual void SetDefaultParameters(CharacterStats stats)
        {
            Name = stats.Name;
        }

        public virtual void NotifyRespawn() => Respawning?.Invoke(this, EventArgs.Empty); 
    }
}
