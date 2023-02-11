using Platformer.UI.MenuSystem.Commands.Base;
using UnityEngine;

namespace Platformer.UI.MenuSystem.Commands.Common
{
	public class EnterMenu : MenuCommand
	{
        [SerializeField]
        private MenuComponent _menuToEnter;

        public override void Execute(object data) =>
            _menuToEnter.Enter(ParentMenu);
	}
}