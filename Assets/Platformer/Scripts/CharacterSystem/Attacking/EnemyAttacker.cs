using Platformer.CharacterSystem.Base;
using Platformer.PlayerSystem;
using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public class EnemyAttacker : Attacker
	{
        public override void OnAttackInput()
        {
            StartAttack();
        }

        protected override IDamagable GetEnemyComponent(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                return player;
            }
            return null;
        }
    }
}