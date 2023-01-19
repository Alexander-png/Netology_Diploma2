using UnityEngine;

namespace Platformer.CharacterSystem.DataContainers
{
	public class CharacterDataContainer
	{
		public string Name { get; set; }
		public Vector3 Position { get; set; }
	}

	public class PlayerDataContainer : CharacterDataContainer
    {
		public float CurrentHealth { get; set; }
    }
}