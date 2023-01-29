using Platformer.GameCore;
using Platformer.PlayerSystem;
using UnityEngine;
using Zenject;

namespace Platformer.LevelEnvironment.Triggers
{
	public class CheckPoint : MonoBehaviour
	{
        [Inject]
        private GameSystem _gameSystem;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Player _))
            {
                _gameSystem.PerformAutoSave();
            }
        }

#if UNITY_EDITOR    
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 0f, 1f, 0.6f);
            Gizmos.DrawCube(transform.position, new Vector3(1, 2, 1));
        }
#endif
    }
}