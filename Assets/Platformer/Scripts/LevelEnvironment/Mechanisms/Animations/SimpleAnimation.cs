using UnityEngine;

namespace Platformer.Scripts.LevelEnvironment.Mechanisms.Animations
{
    public abstract class SimpleAnimation : MonoBehaviour
    {
        [SerializeField]
        private float _animationSpeed;

        protected float AnimationSpeed => _animationSpeed;

        public abstract float AnimationTime { get; }

        public virtual void InitState(bool value) { }
        public virtual void SetSwitched(bool value) { }
    }
}
