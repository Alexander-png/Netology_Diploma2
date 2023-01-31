using Platformer.GameCore;
using UnityEngine;
using Zenject;

namespace Platformer.Interaction
{
    public class GetSkillTrigger : InteractableTrigger
    {
        [Inject]
        private GameSystem _gameSystem;

        [SerializeField]
        private string _skillId;

        public override bool CanInteract => !_gameSystem.CheckSkillAdded(_skillId);

        public override void Interact()
        {
            _gameSystem.AddSkillToPlayer(_skillId);
            InvokeInteracted();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            BoxCollider triggerCollider = GetComponent<BoxCollider>();

            Color c = Color.cyan;
            c.a = 0.6f;
            Gizmos.color = c;

            Gizmos.DrawCube(transform.position, triggerCollider.size);
        }
#endif
    }
}