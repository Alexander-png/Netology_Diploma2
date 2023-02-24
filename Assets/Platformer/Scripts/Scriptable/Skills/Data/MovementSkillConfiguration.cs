using Platformer.CharacterSystem.StatsData;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Data
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Movement skill")]
	public class MovementSkillConfiguration : SkillConfiguration<MovementStatsData>
	{
		[SerializeField]
		private float _maxSpeed;
		[SerializeField]
		private float _acceleration;
		[SerializeField]
		private int _jumpCountInRow;
		[SerializeField]
		private float _climbForce;
		[SerializeField]
		private float _wallClimbRepulsion;
        [SerializeField]
		private float _dashDuration;
		[SerializeField]
		private float _dashRechargeTime;

		public override MovementStatsData GetData() => new MovementStatsData()
		{
			MaxSpeed = _maxSpeed,
			Acceleration = _acceleration,
			JumpCountInRow = _jumpCountInRow,
			ClimbForce = _climbForce,
			WallClimbRepulsion = _wallClimbRepulsion,
			DashDuration = _dashDuration,
			DashRechargeTime = _dashRechargeTime,
		};
	}
}