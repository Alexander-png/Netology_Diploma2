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
        public float ClimbForce;
        public float WallClimbRepulsion;
        public float DashForce;
        public float DashDuration;
        public float DashRechargeTime;
        public float VerticalDashDelimeter;

        public float MaxJumpForce => Jumps.Max();

        public float GetJumpForce(int jumpsLeft)
        {
            if (jumpsLeft == 0)
            {
                return 0;
            }
            return Jumps[Jumps.Count - jumpsLeft];
        }

        public static MovementSkillData operator +(MovementSkillData first, MovementSkillData second)
        {
            var result = new MovementSkillData();
            result.Acceleration = first.Acceleration + second.Acceleration;
            result.MaxSpeed = first.MaxSpeed + second.MaxSpeed;
            result.InAirDrag = first.InAirDrag + second.InAirDrag;

            var longerList = first.Jumps.Count > second.Jumps.Count ? first.Jumps : second.Jumps;
            var shorterList = first.Jumps.Count > second.Jumps.Count ? second.Jumps : first.Jumps;
            result.Jumps = new List<float>(longerList);
            for (int i = 0; i < shorterList.Count; i++)
            {
                result.Jumps[i] += shorterList[i];
            }

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

            var longerList = first.Jumps.Count > second.Jumps.Count ? first.Jumps : second.Jumps;
            var shorterList = first.Jumps.Count > second.Jumps.Count ? second.Jumps : first.Jumps;
            result.Jumps = new List<float>(longerList);
            for (int i = 0; i < shorterList.Count; i++)
            {
                result.Jumps[i] -= shorterList[i];
                if (result.Jumps[i] < 0)
                {
                    result.Jumps[i] = 0;
                }
            }

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
        private float _inAirDrag;
        [SerializeField]
        private float[] _jumpsInRow;
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
        private float _verticalDashDelimeter;

		public override MovementSkillData GetData() => new MovementSkillData()
		{
			MaxSpeed = _maxSpeed,
			Acceleration = _acceleration,
            InAirDrag = _inAirDrag,
            Jumps = new List<float>(_jumpsInRow),
            ClimbForce = _climbForce,
			WallClimbRepulsion = _wallClimbRepulsion,
            DashForce = _dashForce,
			DashDuration = _dashDuration,
			DashRechargeTime = _dashRechargeTime,
            VerticalDashDelimeter = _verticalDashDelimeter,
		};
	}
}