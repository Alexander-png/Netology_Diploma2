using Platformer.PlayerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Platformer.GameCore
{
	public class LevelCompleteNotifier : MonoBehaviour
	{
		[Inject]
		private GameSystem _gameSystem;

        [SerializeField]
        private bool _levelEndTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Player _))
            {
                if (_levelEndTrigger)
                {
                    _gameSystem.OnLevelCompleted(SceneManager.GetActiveScene().name);

                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color c = _levelEndTrigger ? Color.green : Color.red;
            c.a = 0.6f;
            Gizmos.color = c;
            BoxCollider trigger = GetComponent<BoxCollider>();
            Gizmos.DrawCube(transform.position + trigger.center, trigger.size);
        }
#endif
    }
}