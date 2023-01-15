using Platformer.PlayerSystem;
using UnityEngine;

namespace Platformer.LevelEnvironment.Triggers
{
	public class AnimationTrigger : MonoBehaviour
	{
        [SerializeField]
        private Animator _targetAnimator;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _targetAnimator.SetFloat("Begin", 1f);
            }
        }
    }
}