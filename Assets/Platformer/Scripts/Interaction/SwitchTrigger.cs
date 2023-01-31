using UnityEngine;

namespace Platformer.Interaction
{
    public interface ISwitcher : IPerformer
    {
        public bool CanPerform { get; }
        public bool IsSwitchedOn { get; set; }
    }

	public class SwitchTrigger : InteractableTrigger
    {
		[SerializeField]
		private GameObject _targetSwitcher;

        private ISwitcher _switcher;

        public override bool CanInteract => _switcher.CanPerform;

        private void Start()
        {
            _switcher = _targetSwitcher.GetComponent<ISwitcher>();
        }

        public override void Interact()
        {
            _switcher.IsSwitchedOn = !_switcher.IsSwitchedOn;
            InvokeInteracted();
        }
    }
}