using Platformer.CharacterSystem.NPC;
using UnityEngine;

namespace Platformer.Interaction
{
    public interface ITalkable : IPerformer
    {
        public void Talk();
        public void SetConversation(string id, bool reload);
    }

    public class TalkNPCTrigger : InteractableTrigger
    {
        [SerializeField]
        private BaseNPC _targetNPC;

        private ITalkable _talkabkeNPC;

        private void Start()
        {
            _talkabkeNPC = _targetNPC.GetComponent<ITalkable>();
        }

        public override void Interact()
        {
            _talkabkeNPC.Talk();
            InvokeInteracted();
        }
    }
}