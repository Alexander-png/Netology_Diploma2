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

        private MenuComponent _parentMenu;

		protected MenuComponent ParentMenu => _parentMenu;

		public virtual void SetParentMenu(MenuComponent menu) =>
			_parentMenu = menu;

		public virtual void Execute(object data) { }
	}
}