using Platformer.Interactables.Elements.Traps;
using Platformer.LevelEnvironment.Switchers;
using UnityEngine;

namespace Platformer.LevelEnvironment.Elements.Traps.StoneStomper
{
	public class StoneStomperHandler : TrapHandler, ISwitchTarget
	{
        [SerializeField]
        private StomperTrigger[] _stomperTriggers;

        public override bool TrapEnabled
        {
            get => _stomperTriggers.Length != 0 ? _stomperTriggers[0].TrapEnabled : false;
            set
            {
                foreach (var trigger in _stomperTriggers)
                {
                    trigger.TrapEnabled = value;
                }
            }
        }

        public bool IsSwitchedOn
        {
            get => TrapEnabled; 
            set => TrapEnabled = value;
        }

        public float SwitchTime => 0;

        public Transform FocusPoint => transform;

        public void InitState(bool swithcedOn) => IsSwitchedOn = swithcedOn;

#if UNITY_EDITOR
        [ContextMenu("Find stomper triggers")]
        private void FindTriggers()
        {
            _stomperTriggers = FindObjectsOfType<StomperTrigger>();
        }
#endif
    }
}