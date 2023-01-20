using System.Collections.Generic;
using UnityEngine;

namespace Platformer.PlayerSystem
{
    public interface IInventoryItem 
    {
        public string ItemId { get; }
    }

    public class Inventory : MonoBehaviour
    {
        private List<string> _items;

        public IEnumerable<string> Items => new List<string>(_items);

        private void Awake() =>
            _items = new List<string>();

        public void AddItem(string itemId) =>
            _items.Add(itemId);

        public bool RemoveItem(string itemToRemoveId, int count = 1)
        {
            List<string> itemsToRemove = _items.FindAll(i => i == itemToRemoveId);
            for (int i = 0; i < count; i++)
            {
                _items.Remove(itemsToRemove[i]);
            }
            return itemsToRemove.Count > 0;
        }

        public bool ContainsItem(string itemId, int count) => 
            _items.FindAll(i => i == itemId).Count >= count;
    }
}
