using UnityEngine;

namespace Platformer.Scriptable.Characters
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Character stats")]
	public class CharacterStats : ScriptableObject
	{
		// TODO: split this class by character types
		[SerializeField]
		private string _name;
		[SerializeField]
		private float _maxHealth;
		[SerializeField]
		private float _damageImmuneTime;
		[SerializeField]
		private bool _immortal;
		[SerializeField]
		private bool _interactable;
		
		public string Name => _name;
		public float MaxHealth => _maxHealth;
		public float DamageImmuneTime => _damageImmuneTime;
		public bool Immortal => _immortal;
		public bool Interactable => _interactable;
    }
}