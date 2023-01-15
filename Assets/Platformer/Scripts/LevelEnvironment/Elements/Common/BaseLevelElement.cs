using Platformer.GameCore;
using UnityEngine;
using Zenject;

namespace Platformer.LevelEnvironment.Elements.Common
{
	public class BaseLevelElement : MonoBehaviour
	{
        [Inject]
        private GameSystem _gameSystem;

        public GameSystem GameSystem => _gameSystem;
    }
}