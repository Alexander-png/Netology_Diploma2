using Platformer.GameCore;
using Platformer.Interaction;
using Platformer.QuestSystem;
using UnityEngine;

namespace Platformer.CharacterSystem.NPC
{
	public class TalkableNPC : BaseNPC, ITalkable, IQuestGiver, IQuestTarget
    {
        [SerializeField]
        private string _conversationId;
        [SerializeField]
        private string _npcId;

        public string QuestTargetId => _npcId;

        public string ConversationId 
        {
            get => _conversationId; 
            set => _conversationId = value;
        }

        public void Talk()
        {
            //GameSystem.ConversationHandler.StartConversation(_conversationId);
        }

        public void SetConversation(string id, bool hotReload = false)
        {
            _conversationId = id;
            if (hotReload)
            {
                Talk();
            }
        }
    }
}