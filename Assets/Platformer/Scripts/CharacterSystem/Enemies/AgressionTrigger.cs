using Platformer.PlayerSystem;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
	public class AgressionTrigger : MonoBehaviour
	{
        [SerializeField]
		private Enemy _owner;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _owner.OnPlayerNearby();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _owner.OnPlayerRanAway();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color c = Color.gray;
            c.a = 0.3f;
            Gizmos.color = c;

            BoxCollider collider = GetComponent<BoxCollider>();

            if (collider != null)
            {
                Vector3 rangeVisualSize = collider.size;
                Gizmos.DrawCube(transform.position + collider.center, rangeVisualSize);
            }
        }
#endif
    }
}