using Platformer.GameCore;
using Platformer.Interaction;
using UnityEngine;
using Zenject;

namespace Platformer.LevelEnvironment.Mechanisms.Switchers
{
    public abstract class Switch : MonoBehaviour, ISwitcher
	{
		[Inject]
		private GameSystem _gameSystem;

		[SerializeField]
		protected bool _isSwitchedOn;
		[SerializeField]
		private bool _isOneOff;

		public GameSystem GameSystem => _gameSystem;

		public abstract bool IsSwitchedOn { get; set; }

		public bool WasSwitched { get; protected set; }
		public bool IsOneOff => _isOneOff;
		public virtual bool CanPerform => !(WasSwitched && _isOneOff);

		protected virtual void Start() { }
	}
}