using Platformer.GameCore;
using Platformer.Interaction;
using Platformer.PlayerSystem;
using Platformer.QuestSystem;
using UnityEngine;
using Zenject;

namespace Platformer.Collectables
{
    public class CollectableItem : InteractableTrigger, IInventoryItem, IQuestTarget
    {
        [Inject]
        protected GameSystem _gameSystem;

        [SerializeField]
        private string _itemId;
        [SerializeField]
        private bool _autoCollect;

        private bool _collected;

        public string ItemId => _itemId;
        public string QuestTargetId => ItemId;

        public override void Interact()
        {
            if (_collected)
            {
                return;
            }

            Collect();
            InvokeInteracted();
            _collected = true;
        }

        protected virtual void Collect()
        {
            _gameSystem.OnItemCollected(this);
            gameObject.SetActive(false);
            CanInteract = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if (other.TryGetComponent(out Player _))
            {
                if (_autoCollect)
                {
                    Interact();
                }
            }
        }
    }
}