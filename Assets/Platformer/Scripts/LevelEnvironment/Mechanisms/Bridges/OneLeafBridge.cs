using Platformer.Scripts.LevelEnvironment.Mechanisms.Animations;
using UnityEngine;

namespace Platformer.LevelEnvironment.Mechanisms.Bridges
{
    public class OneLeafBridge : Drawbridge
    {
        [SerializeField]
        private SimpleAnimation _animation;

        private bool _isSwitchedOn;

        public override bool IsSwitchedOn
        {
            get => _isSwitchedOn;
            set
            {
                _isSwitchedOn = value;
                if (_animation != null) _animation.SetSwitched(value);
            }
        }

        public override float SwitchTime => _animation != null ? _animation.AnimationTime : 0f;

        public override void InitState(bool swithcedOn) =>
            _animation?.InitState(swithcedOn);

        protected virtual void Start()
        {
            if (_animation == null)
            {
                EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: animation not specified.", EditorExtentions.GameLogger.LogType.Warning);
                return;
            }
            _animation.InitState(_switchedByDefault);
        }
    }
}