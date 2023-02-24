using System;
using UnityEngine;

namespace Platformer.Scriptable.EntityConfig
{
	// TODO: replace default stats with default skills.
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Character stats")]
    [Obsolete("Must remove this class. Use Character skills instead")]
	public class CharacterStats : ScriptableObject
	{
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