using UnityEngine;

namespace Platformer.Scriptable.Characters
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/EnemyBehaviourConfig")]
	public class EnemyBehaviourConfig : ScriptableObject
	{
        [SerializeField]
        private float _idleTime;
        [SerializeField]
        private float _attackRadius;
        [SerializeField]
        private float _playerHeightDiffToJump;
        [SerializeField]
        private float _closeToPlayerDistance;
        [SerializeField]
        private float _argressionRadius;

        public float IdleTime => _idleTime;
        public float AttackRadius => _attackRadius;
        public float PlayerHeightDiffToJump => _playerHeightDiffToJump;
        public float CloseToPlayerDistance => _closeToPlayerDistance;
        public float ArgressionRadius => _argressionRadius;
    }
}