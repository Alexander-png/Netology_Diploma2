using UnityEngine;

namespace Platformer.Scriptable.Collectables
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/HealPackConfig")]
	public class HealPackConfig : ScriptableObject
	{
		[SerializeField]
		private float _healValue;

		public float HealValue => _healValue;
	}
}