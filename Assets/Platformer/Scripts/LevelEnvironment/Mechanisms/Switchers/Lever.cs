using Platformer.LevelEnvironment.Switchers;
using Platformer.Scripts.LevelEnvironment.Mechanisms.Animations;
using UnityEngine;

namespace Platformer.LevelEnvironment.Mechanisms.Switchers
{
	public class Lever : Switch
	{
		[SerializeField]
		private SimpleAnimation _switchAnimator;

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
					_switchAnimator.SetSwitched(value);
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
			if (Target == null)
            {
                EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: target not specified.", EditorExtentions.GameLogger.LogType.Warning);
                return;
            }

			_switchTarget = Target.GetComponent<ISwitchTarget>();
            if (_switchTarget == null)
            {
                EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: the {Target.name} does not contain ISwitcherTarget component.", EditorExtentions.GameLogger.LogType.Error);
				return;
            }

			_switchTarget.InitState(IsSwitchedOn);

			if (_switchAnimator == null)
            {
				EditorExtentions.GameLogger.AddMessage($"{gameObject.name}: animation not specified.", EditorExtentions.GameLogger.LogType.Warning);
				return;
			}
			_switchAnimator.InitState(IsSwitchedOn);
        }
    }
}