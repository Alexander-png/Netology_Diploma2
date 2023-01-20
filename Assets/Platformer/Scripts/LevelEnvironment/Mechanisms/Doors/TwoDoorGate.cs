using Platformer.LevelEnvironment.Mechanisms.Animations;
using Platformer.LevelEnvironment.Switchers;
using UnityEngine;

namespace Platformer.LevelEnvironment.Mechanisms.Doors
{
	public class TwoDoorGate : Gate, ISwitcherTarget
	{
		[SerializeField]
		private TwoDoorAnimation _animation;

		private bool _isOpened;

        public override bool IsOpened
		{
			get => _isOpened; 
			set
			{
				_isOpened = value;
				if (_animation != null) _animation.SetOpened(value);
			}
		}

        public bool IsSwitchedOn 
		{
			get => IsOpened; 
			set => IsOpened = value;
		}

        public float SwitchTime => _animation != null ? _animation.AnimationTime : 0f;

        public Transform FocusPoint => CameraFocusPoint;

        private void Start()
        {
			if (_animation == null)
			{
				EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: animation not specified.", EditorExtentions.GameLogger.LogType.Warning);
				return;
			}
			_animation.InitState(_openedByDefault);
		}
    }
}