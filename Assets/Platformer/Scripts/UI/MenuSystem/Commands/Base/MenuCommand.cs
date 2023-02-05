using Platformer.GameCore;
using UnityEngine;
using Zenject;

namespace Platformer.UI.MenuSystem.Commands.Base
{
    [System.Serializable]
	public class MenuCommand : MonoBehaviour
	{
		[Inject]
		protected GameSystem _gameSystem;

        private MenuComponent _associatedMenu;

		protected MenuComponent AssociatedMenu => _associatedMenu;

		public virtual void SetAssociatedMenu(MenuComponent menu) =>
			_associatedMenu = menu;

		public virtual void Execute() { }
	}
}