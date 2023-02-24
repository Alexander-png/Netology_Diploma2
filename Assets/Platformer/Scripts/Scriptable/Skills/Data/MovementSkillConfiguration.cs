using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Data
{
    public struct MovementSkillData
    {
        public float Acceleration;
        public float MaxSpeed;
        public float InAirDrag;
        public List<float> Jumps;
        public int JumpCountInRow;
        public float ClimbForce;
        public float WallClimbRepulsion;
        public float DashForce;
        public float DashDuration;
        public float DashRechargeTime;
        public float VerticalDashDelimeter;

        public float MaxJumpForce => Jumps.Max();

        public float GetJumpForce(int jumpsLeft)
        {
            int counter = JumpCountInRow;
            if (counter > Jumps.Count)
            {
                counter = Jumps.Count;
                EditorExtentions.GameLogger.AddMessage("Jumps count in row is larger than count of defined jump forces. Please check default player movement configuration and/or applied skills", EditorExtentions.GameLogger.LogType.Error);
            }
            int index = counter - jumpsLeft;
            if (index < 0)
            {
                index = 0;
            }
            return Jumps[index];
            //return Jumps[JumpCountInRow - jumpsLeft];
        }

        public static MovementSkillData operator +(MovementSkillData first, MovementSkillData second)
        {
            var result = new MovementSkillData();
            result.Acceleration = first.Acceleration + second.Acceleration;
            result.MaxSpeed = first.MaxSpeed + second.MaxSpeed;
            result.InAirDrag = first.InAirDrag + second.InAirDrag;
            result.JumpCountInRow = first.JumpCountInRow + second.JumpCountInRow;
            result.Jumps = new List<float>(first.Jumps);
            result.ClimbForce = first.ClimbForce + second.ClimbForce;
            result.WallClimbRepulsion = first.WallClimbRepulsion + second.WallClimbRepulsion;
            result.DashForce = first.DashForce + second.DashForce;
            result.DashDuration = first.DashDuration + second.DashDuration;
            result.DashRechargeTime = first.DashRechargeTime + second.DashRechargeTime;
            result.VerticalDashDelimeter = first.VerticalDashDelimeter + second.VerticalDashDelimeter;
            return result;
        }

        public static MovementSkillData operator -(MovementSkillData first, MovementSkillData second)
        {
            var result = new MovementSkillData();
            result.Acceleration = first.Acceleration - second.Acceleration;
            result.MaxSpeed = first.MaxSpeed - second.MaxSpeed;
            result.InAirDrag = first.InAirDrag - second.InAirDrag;
            result.JumpCountInRow = first.JumpCountInRow - second.JumpCountInRow;
            if (result.JumpCountInRow < 0)
            {
#if UNITY_EDITOR
                EditorExtentions.GameLogger.AddMessage("Jump count in row is lower than zero! Something went wrong!", EditorExtentions.GameLogger.LogType.Fatal);
#elif UNITY_STANDALONE
                result.JumpCountInRow = 0;
#endif
            }
            result.Jumps = new List<float>(first.Jumps);
            result.ClimbForce = first.ClimbForce - second.ClimbForce;
            result.WallClimbRepulsion = first.WallClimbRepulsion - second.WallClimbRepulsion;
            result.DashForce = first.DashForce - second.DashForce;
            result.DashDuration = first.DashDuration - second.DashDuration;
            result.DashRechargeTime = first.DashRechargeTime - second.DashRechargeTime;
            result.VerticalDashDelimeter = first.VerticalDashDelimeter - second.VerticalDashDelimeter;
            return result;
        }
    }

    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Movement skill")]
	public class MovementSkillConfiguration : SkillConfiguration<MovementSkillData>
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

		public override MovementSkillData GetData() => new MovementSkillData()
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