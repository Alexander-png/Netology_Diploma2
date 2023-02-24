using Platformer.CharacterSystem.StatsData;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Scriptable.EntityConfig
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Movement stats")]
	public class MovementStats : ScriptableObject
	{
		[SerializeField]
		private float _maxSpeed;
		[SerializeField]
		private float _acceleration;
		[SerializeField]
		private float _inAirDrag;
		[SerializeField]
		private float[] _jumps;
		[SerializeField]
		private int _jumpCountInRow;
		[SerializeField]
		private float _climbForce;
		[SerializeField]
		private float _wallClimbRepulsion;
		[SerializeField]
		private float _dashForce;
		[SerializeField]
		private float _dashDuration;
		[SerializeField]
		private float _dashRechargeTime;
		[SerializeField]
		private float _verticalDashDelimeter = 1f;

		public MovementStatsData GetData() => new MovementStatsData()
		{
			MaxSpeed = _maxSpeed,
			Acceleration = _acceleration,
			InAirDrag = _inAirDrag,
			Jumps = new List<float>(_jumps), // need for jumps initialization
			JumpCountInRow = _jumpCountInRow,
			ClimbForce = _climbForce,
			WallClimbRepulsion = _wallClimbRepulsion,
			DashForce = _dashForce,
			DashDuration = _dashDuration,
			DashRechargeTime = _dashRechargeTime,
			VerticalDashDelimeter = _verticalDashDelimeter,
		};
	}
}