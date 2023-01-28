using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public abstract class Attacker : MonoBehaviour
	{
		[SerializeField]
		protected Transform _ownerTransform;
	}
}