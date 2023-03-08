using System;
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

        public bool IsProportion;

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
            if (first.IsProportion == second.IsProportion)
            {
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
            }
            else if (!first.IsProportion && second.IsProportion)
            {
                result.Acceleration = first.Acceleration + (first.Acceleration * second.Acceleration);
                result.MaxSpeed = first.MaxSpeed + (first.MaxSpeed * second.MaxSpeed);
                result.InAirDrag = first.InAirDrag + (first.InAirDrag * second.InAirDrag);

                result.Jumps = new List<float>(first.Jumps);
                for (int i = 0; i < result.Jumps.Count && i < second.Jumps.Count; i++)
                {
                    result.Jumps[i] += result.Jumps[i] * second.Jumps[i];
                }

                result.ClimbForce = first.ClimbForce + (first.ClimbForce * second.ClimbForce);
                result.WallClimbRepulsion = first.WallClimbRepulsion + (first.WallClimbRepulsion * second.WallClimbRepulsion);
                result.DashForce = first.DashForce + (first.DashForce * second.DashForce);
                result.DashDuration = first.DashDuration + (first.DashDuration * second.DashDuration);
                result.DashRechargeTime = first.DashRechargeTime + (first.DashRechargeTime * second.DashRechargeTime);
                result.VerticalDashDelimeter = first.VerticalDashDelimeter + (first.VerticalDashDelimeter * second.VerticalDashDelimeter);
            }
            else
            {
                throw new InvalidOperationException("Addition of absolute values to propotion is not supported.");
            }
            return result;
        }

        public static MovementSkillData operator -(MovementSkillData first, MovementSkillData second)
        {
            var result = new MovementSkillData();
            if (first.IsProportion == second.IsProportion)
            {
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
            }
            else if (!first.IsProportion && second.IsProportion)
            {
                result.Acceleration = first.Acceleration - (first.Acceleration * second.Acceleration);
                result.MaxSpeed = first.MaxSpeed - (first.MaxSpeed * second.MaxSpeed);
                result.InAirDrag = first.InAirDrag - (first.InAirDrag * second.InAirDrag);

                result.Jumps = new List<float>(first.Jumps);
                for (int i = 0; i < result.Jumps.Count && i < second.Jumps.Count; i++)
                {
                    result.Jumps[i] -= result.Jumps[i] * second.Jumps[i];
                    if (result.Jumps[i] < 0)
                    {
                        result.Jumps[i] = 0;
                    }
                }

                result.ClimbForce = first.ClimbForce - (first.ClimbForce * second.ClimbForce);
                result.WallClimbRepulsion = first.WallClimbRepulsion - (first.WallClimbRepulsion * second.WallClimbRepulsion);
                result.DashForce = first.DashForce - (first.DashForce * second.DashForce);
                result.DashDuration = first.DashDuration - (first.DashDuration * second.DashDuration);
                result.DashRechargeTime = first.DashRechargeTime - (first.DashRechargeTime * second.DashRechargeTime);
                result.VerticalDashDelimeter = first.VerticalDashDelimeter - (first.VerticalDashDelimeter * second.VerticalDashDelimeter);
            }
            else
            {
                throw new InvalidOperationException("Substraction of absolute values to propotion is not supported.");
            }
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
            Jumps = new List<float>(_jumpsInRow ?? new float[0]),
            ClimbForce = _climbForce,
			WallClimbRepulsion = _wallClimbRepulsion,
            DashForce = _dashForce,
			DashDuration = _dashDuration,
			DashRechargeTime = _dashRechargeTime,
            VerticalDashDelimeter = _verticalDashDelimeter,
            IsProportion = IsProprotion,
        };
	}
}