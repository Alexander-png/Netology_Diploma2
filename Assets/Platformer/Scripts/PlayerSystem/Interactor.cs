using Platformer.GameCore;
using Platformer.Interaction;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Platformer.PlayerSystem
{
    public class Interactor : MonoBehaviour
    {
        [Inject]
        private GameSystem _gameSystem;
        
        private bool _canInteract;
        private Player _player;

        protected virtual void Start() =>
            _player = _gameSystem.GetPlayer();

        public InteractableTrigger CurrentTrigger
        {
            get => _gameSystem.CurrentTrigger;
            set
            {
                _gameSystem.CurrentTrigger = value;
                if (value != null && value.CanInteract)
                {
                    StopAllCoroutines();
                    _canInteract = true;
                }

                if (value == null)
                {
                    _canInteract = false;
                    StopAllCoroutines();
                }
            }
        }

        public bool HandlingEnabled { get; set; } = true;

        public void OnInteractInput()
        {
            if (_canInteract && HandlingEnabled)
            {
                if (CurrentTrigger.NeedStop)
                {
                    _player.MovementController.Velocity = Vector3.zero;
                }
                CurrentTrigger.Interact();
            }
        }
    }
}