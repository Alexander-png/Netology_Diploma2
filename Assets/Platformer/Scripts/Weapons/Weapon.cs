using Platformer.CharacterSystem.Base;
using UnityEngine;

namespace Platformer.Weapons
{
	public abstract class Weapon : MonoBehaviour
	{
		[SerializeField]
		protected Entity _owner;
	}
}