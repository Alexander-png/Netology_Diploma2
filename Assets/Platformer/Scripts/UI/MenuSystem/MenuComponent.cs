using Platformer.UI.MenuSystem.Items;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Platformer.EditorExtentions;
using Platformer.UI.MenuSystem.Commands.Base;
using RotaryHeart.Lib.SerializableDictionary;

namespace Platformer.UI.MenuSystem
{
    [Serializable]
    public class MenuCommandDictionary : SerializableDictionaryBase<string, MenuCommand> { }

    [RequireComponent((typeof(PlayerInput)))]
    public class MenuComponent : MonoBehaviour
    {
        [SerializeField]
        private MenuItem[] _items;
        [SerializeField]
        private MenuCommandDictionary _commandBindings;

        private MenuComponent _previousMenu;

        private int _selectionIndex = 0;

        protected MenuItem[] MenuItems
        {
            get
            {
                MenuItem[] toReturn = new MenuItem[_items.Length];
                Array.Copy(_items, toReturn, _items.Length);
                return toReturn;
            }
        }

        protected int SelectionIndex => _selectionIndex;

        private void Start() =>
            Initialize();

        protected virtual void Initialize()
        {
            FindAndSortMenuItems();

            if (_items?.Length != 0)
            {
                try
                {
                    MenuItem selectedMarker = _items.First(i => i.IsSelected);
                    _selectionIndex = Array.IndexOf(_items, selectedMarker);
                    foreach (MenuItem item in _items)
                    {
                        item.SetParent(this);
                    }
                }
                catch (InvalidOperationException exc)
                {
                    GameLogger.AddMessage(exc.Message, GameLogger.LogType.Error);
                }
            }

            foreach (var binding in _commandBindings)
            {
                binding.Value.SetParentMenu(this);
            }
        }

        private void FindAndSortMenuItems()
        {
            try
            {
                _items = GetComponentsInChildren<MenuItem>();
                Array.Sort(_items, new Comparison<MenuItem>((item1, item2) => item1.SelectionIndex.CompareTo(item2.SelectionIndex)));
            }
            catch (InvalidOperationException)
            {
                GameLogger.AddMessage("No selected item found, or no menu items found!", GameLogger.LogType.Error);
            }
        }

        protected void OnSelectionChanging(InputValue value)
        {
            int newIndex = _selectionIndex + Convert.ToInt32(value.Get<float>());

            if (newIndex == _selectionIndex)
            {
                return;
            }

            _items[_selectionIndex].IsSelected = false;
            _selectionIndex = newIndex;

            if (_selectionIndex < 0)
            {
                _selectionIndex = _items.Length - 1;
            }
            if (_selectionIndex > _items.Length - 1)
            {
                _selectionIndex = 0;
            }
            _items[_selectionIndex].IsSelected = true;
        }

        public void OnItemPointerEntered(string commandId)
        {
            var item = _items.First(i => i.CommandId == commandId);
            _items[_selectionIndex].IsSelected = false;
            _selectionIndex = item.SelectionIndex;
            _items[_selectionIndex].IsSelected = true;
        }

        public void OnPerform(InputValue value) =>
            _commandBindings[_items[_selectionIndex].CommandId].Execute(_items[_selectionIndex].Data);

        public void OnItemPointerClicked() => 
            OnPerform(null);

        public void Enter(MenuComponent previousMenu)
        {
            _previousMenu = previousMenu;
            _previousMenu.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public void Exit()
        {
            if (_previousMenu == null)
            {
                GameLogger.AddMessage("No previous menu specified. Can not exit.", GameLogger.LogType.Error);
                return;
            }
            _previousMenu.gameObject.SetActive(true);
            _previousMenu = null;
            gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        [ContextMenu("Find menu items")]
        private void FindMenuItems()
        {
            FindAndSortMenuItems();
            FillCommandBindings();
        }

        private void FillCommandBindings()
        {
            _commandBindings = new MenuCommandDictionary();
            foreach (var item in _items)
            {
                _commandBindings[item.CommandId] = null;
            }
        }
#endif
    }
}

