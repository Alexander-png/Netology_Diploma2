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

		[SerializeField]
		private Transform _cameraFocusPoint;

		protected Transform CameraFocusPoint => _cameraFocusPoint;
		protected GameSystem GameSystem => _gameSystem;

		public abstract bool IsOpened { get; set; }

		protected virtual void OnDrawGizmos()
        {
			if (CameraFocusPoint == null)
			{
				return;
			}

			Color color = Color.cyan;
			color.a = 0.5f;
			Gizmos.color = color;
			Gizmos.DrawSphere(CameraFocusPoint.position, 1f);
		}
    }
}