using Platformer.GameCore;
using UnityEngine;
using Zenject;

namespace Platformer.LevelEnvironment.Mechanisms.Doors
{
    public abstract class Gate : MonoBehaviour
	{
        [Inject]
		private GameSystem _gameSystem;

		[SerializeField, Tooltip("Notice: if this object is handled by another object, this field will not work.")]
		protected bool _openedByDefault;

		protected GameSystem GameSystem => _gameSystem;

		public abstract bool IsOpened { get; set; }
    }
}