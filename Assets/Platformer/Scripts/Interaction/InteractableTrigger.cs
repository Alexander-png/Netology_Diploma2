using Platformer.PlayerSystem;
using System;
using UnityEngine;

namespace Platformer.Interaction
{
    public interface IPerformer
    {

    }

    public abstract class InteractableTrigger : MonoBehaviour
	{
        //[SerializeField]
        //private float _interactionDelay;
        [SerializeField]
        private bool _needStop;

        public event EventHandler Interacted;

		public virtual bool CanInteract { get; } = true;
		public abstract void Interact();

        //public float InteractionDelay => _interactionDelay;
        public bool NeedStop => _needStop;

        protected void InvokeInteracted() => 
            Interacted?.Invoke(this, EventArgs.Empty);

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Interactor interactor))
            {
                interactor.CurrentTrigger = this;
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Interactor interactor))
            {
                if (interactor.CurrentTrigger == this)
                {
                    interactor.CurrentTrigger = null;
                }
            }
        }
    }
}