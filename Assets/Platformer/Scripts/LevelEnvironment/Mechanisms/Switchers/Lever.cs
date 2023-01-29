using Platformer.LevelEnvironment.Mechanisms.Animations;
using Platformer.LevelEnvironment.Switchers;
using UnityEngine;

namespace Platformer.LevelEnvironment.Mechanisms.Switchers
{
	public class Lever : Switch
	{
		[SerializeField]
		private GameObject _target;
		[SerializeField]
		private LeverSwitchAnimation _switchAnimator;

		private ISwitchTarget _switchTarget;

		public override bool IsSwitchedOn
		{
			get => _isSwitchedOn;
			set
			{
				if (!CanPerform)
                {
					return;
                }

				if (_isSwitchedOn != value)
                {
					WasSwitched = true;
				}

				_isSwitchedOn = value;

				if (_switchAnimator != null)
                {
					_switchAnimator.Switch(value);
                }
				if (_switchTarget != null) 
				{
					_switchTarget.IsSwitchedOn = value;
				} 
			}
		}

        protected override void Start()
        {
			base.Start();
			if (_target == null)
            {
                EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: target not specified.", EditorExtentions.GameLogger.LogType.Warning);
                return;
            }

			_switchTarget = _target.GetComponent<ISwitchTarget>();
            if (_switchTarget == null)
            {
                EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: the {_target.gameObject.name} does not contain ISwitcherTarget component.", EditorExtentions.GameLogger.LogType.Error);
				return;
            }

			_switchTarget.IsSwitchedOn = IsSwitchedOn;

			if (_switchAnimator == null)
            {
				EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: animation not specified.", EditorExtentions.GameLogger.LogType.Warning);
				return;
			}
			_switchAnimator.InitState(IsSwitchedOn);
        }
    }
}