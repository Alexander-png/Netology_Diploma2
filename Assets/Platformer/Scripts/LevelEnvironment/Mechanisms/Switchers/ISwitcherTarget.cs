using UnityEngine;

namespace Platformer.LevelEnvironment.Switchers
{
    public interface ISwitcherTarget
    {
        public bool IsSwitchedOn { get; set; }
        public float SwitchTime { get; }
        public Transform FocusPoint { get; }
    }
}