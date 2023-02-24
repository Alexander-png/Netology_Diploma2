using UnityEngine;

namespace Platformer.Scriptable.EntityConfig
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
        [SerializeField]
        private float _dashUsageReloadTime;
        [SerializeField]
        private int _dashCountWithoutReload;
        [SerializeField]
        private float _dashChargeTime;
        [SerializeField]
        private float _dashCooldownTime;

        public float IdleTime => _idleTime;
        public float AttackRadius => _attackRadius;
        public float PlayerHeightDiffToJump => _playerHeightDiffToJump;
        public float CloseToPlayerDistance => _closeToPlayerDistance;
        public float ArgressionRadius => _argressionRadius;
        public float DashUsageReloadTime => _dashUsageReloadTime;
        public int DashCountWithoutReload => _dashCountWithoutReload;
        public float DashChargeTime => _dashChargeTime;
        public float DashCooldownTime => _dashCooldownTime;
    }
}