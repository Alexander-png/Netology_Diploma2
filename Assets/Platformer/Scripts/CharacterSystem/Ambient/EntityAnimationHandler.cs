using Platformer.CharacterSystem.Base;
using Platformer.EditorExtentions;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

namespace Platformer.CharacterSystem.Ambient
{
    [Serializable]
    public class EventAnimationParameterDict : SerializableDictionaryBase<EntityEventTypes, string> { }

    public class EntityAnimationHandler : MonoBehaviour
	{
        [SerializeField]
        private EventAnimationParameterDict _parameterMapping;

        private Entity _eventSource;
        private Animator _animator;
        private AnimationListener _animationListner;

        private void Start()
        {
            _eventSource = GetComponent<Entity>();
            if (_eventSource == null)
            {
                GameLogger.AddMessage("No entity found. Animations will not work.", GameLogger.LogType.Error);
                return;
            }
            _eventSource.EventInvoked += OnEntityEvent;
            
            _animator = GetComponentInChildren<Animator>();
            _animationListner = GetComponentInChildren<AnimationListener>();
            if (_animationListner == null)
            {
                GameLogger.AddMessage("Animation listener not found.", GameLogger.LogType.Error);
                return;
            }
            _animationListner.SetListener(OnAnimationEnd);
        }

        private void OnDisable()
        {
            if (_eventSource != null)
            {
                _eventSource.EventInvoked -= OnEntityEvent;
            }
        }

        private void ResetAnimatorState()
        {
            foreach (string paramName in _parameterMapping.Values)
            {
                _animator.SetFloat(paramName, 0);
            }
        }

        private void OnEntityEvent(object sender, EntityEventTypes e)
        {
            ResetAnimatorState();
            if (e == EntityEventTypes.ResetState)
            {
                return;
            }

            string targetParam = string.Empty;
            if (_parameterMapping.TryGetValue(e, out targetParam))
            {
                _animator.SetFloat(targetParam, 1);
            }
            else
            {
                GameLogger.AddMessage($"No animation found for event {e}. Processing event immediately", GameLogger.LogType.Warning);
                _eventSource.OnEventProcessed(e);
            }
        }

        public void OnAnimationEnd(string animationClipName)
        {
            foreach(var pair in _parameterMapping)
            {
                if (pair.Value == animationClipName)
                {
                    _eventSource.OnEventProcessed(pair.Key);
                }
            }
        }
    }
}