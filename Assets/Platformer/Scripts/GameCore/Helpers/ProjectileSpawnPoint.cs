using UnityEngine;

namespace Platformer.GameCore.Helpers
{
	public class ProjectileSpawnPoint : MonoBehaviour 
	{
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color c = Color.yellow;
            c.a = 0.3f;
            Gizmos.color = c;
            Gizmos.DrawSphere(transform.position, 1f);
        }
#endif
    }
}