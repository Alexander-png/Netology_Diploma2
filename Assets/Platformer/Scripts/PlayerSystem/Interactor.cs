using Platformer.GameCore;
using Platformer.Interaction;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Platformer.PlayerSystem
{
    public class Interactor : MonoBehaviour
    {
        [Inject]
        private GameSystem _gameSystem;
        
        private bool _canInteract;
        private Player _player;

        protected virtual void Start()
        {
            _player = _gameSystem.GetPlayer();
        }

        public InteractionTrigger CurrentTrigger
        {
            get => _gameSystem.CurrentTrigger;
            set
            {
                _gameSystem.SetCurrentTrigger(value);
                if (value != null && value.CanPerform)
                {
                    StopAllCoroutines();
                    StartCoroutine(ShowTooltipDelay(value.InteractionDelay));
                }

                if (value == null)
                {
                    _canInteract = false;
                    StopAllCoroutines();
                }
            }
        }

        public bool HandlingEnabled { get; set; } = true;

        public void OnInteract(InputValue value)
        {
            if (_canInteract && HandlingEnabled)
            {
                if (CurrentTrigger.NeedStop)
                {
                    _player.MovementController.Velocity = Vector3.zero;
                }
                _gameSystem.PerformTrigger();
            }
        }

        // TODO: remove delay?
        private IEnumerator ShowTooltipDelay(float time)
        {
            yield return new WaitForSeconds(time);
            //_gameSystem.ShowInteractionTooltip();
            _canInteract = true;
        }
    }
}