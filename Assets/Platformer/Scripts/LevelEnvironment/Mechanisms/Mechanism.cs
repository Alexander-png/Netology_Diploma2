using Platformer.GameCore;
using Platformer.LevelEnvironment.Switchers;
using UnityEngine;
using Zenject;

namespace Platformer.Scripts.LevelEnvironment.Mechanisms
{
    public abstract class Mechanism : MonoBehaviour, ISwitchTarget
	{
		[Inject]
		private GameSystem _gameSystem;

		[SerializeField, Tooltip("Notice: if this object is handled by another object, this field will not work.")]
		protected bool _switchedByDefault;

		protected GameSystem GameSystem => _gameSystem;

        public abstract bool IsSwitchedOn { get; set; }
		public abstract float SwitchTime { get; }

		public abstract void InitState(bool swithcedOn);
    }
}