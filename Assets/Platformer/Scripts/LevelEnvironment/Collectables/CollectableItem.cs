using Platformer.GameCore;
using Platformer.PlayerSystem;
using Platformer.QuestSystem;
using UnityEngine;
using Zenject;

namespace Platformer.LevelEnvironment.Collectables
{
    public class CollectableItem : MonoBehaviour, IInventoryItem, IQuestTarget
    {
        [Inject]
        private GameSystem _gameSystem;

        [SerializeField]
        private string _itemId;

        public string QuestTargetId => _itemId;
        public string ItemId => _itemId;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _gameSystem.OnCollectalbeCollected(this);
                gameObject.SetActive(false);
            }
        }
    }
}