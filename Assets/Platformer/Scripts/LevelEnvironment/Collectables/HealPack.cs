using Platformer.Scriptable.Collectables;
using UnityEngine;

namespace Platformer.LevelEnvironment.Collectables
{
	public class HealPack : CollectableItem
	{
        [SerializeField]
        private HealPackConfig _healConfig;

        protected override void Collect()
        {
            var player = _gameSystem.GetPlayer();
            player.Heal(_healConfig.HealValue);
            base.Collect();
        }
    }
}