using Platformer.PlayerSystem;
using UnityEngine;

namespace Platformer.GameCore.LevelHandlers
{
	public class EscapeStageMarker : MonoBehaviour
	{
		[SerializeField]
		private int _index;
        // TODO: move to better place
        [SerializeField]
        private float _hazardSpeed;

        private EscapeHandler _handler;

        public int Index => _index;
        public float HazardSpeed => _hazardSpeed;

        public void SetHandler(EscapeHandler handler) =>
            _handler = handler;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _handler.OnStageReached(Index);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color c = Color.yellow;
            c.a = 0.3f;
            Gizmos.color = c;

            BoxCollider trigger = GetComponent<BoxCollider>();
            if (trigger != null)
            {
                Gizmos.DrawCube(transform.position + trigger.center, trigger.size);
            }
        }
#endif
    }
}