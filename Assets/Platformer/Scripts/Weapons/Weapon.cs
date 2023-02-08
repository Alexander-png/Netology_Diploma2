using Platformer.CharacterSystem.Base;
using UnityEngine;

namespace Platformer.Weapons
{
	public abstract class Weapon : MonoBehaviour
	{
		protected Entity _owner;

		public void SetDamageToOwner(float damage)
		{
			(_owner as IDamagable).SetDamage(damage, Vector3.zero, true);
		}

		protected void FindOwner()
        {
			if (_owner != null)
            {
				return;
            }
			
			_owner = GetComponentInParent<Entity>();
			if (_owner == null)
			{
				EditorExtentions.GameLogger.AddMessage($"Owner not found, game object name: {gameObject.name}", EditorExtentions.GameLogger.LogType.Error);
			}
		}

        protected virtual void Start() => 
			FindOwner();
    }
}