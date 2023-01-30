namespace Platformer.LevelEnvironment.Switchers
{
    public interface ISwitchTarget
    {
        public bool IsSwitchedOn { get; set; }
        public float SwitchTime { get; }

        public void InitState(bool swithcedOn);
    }
}