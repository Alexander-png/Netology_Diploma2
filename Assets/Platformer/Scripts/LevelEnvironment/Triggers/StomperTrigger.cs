using Platformer.Interactables.Elements.Traps;
using UnityEngine;

namespace Platformer.LevelEnvironment.Triggers
{
    public class StomperTrigger : MonoBehaviour
    {
        [SerializeField]
        private StomperTrap _stone;

        public bool TrapEnabled
        {
            get => _stone.TrapEnabled;
            set => _stone.TrapEnabled = value;
        }

        private void OnTriggerStay(Collider other)
        {
            _stone.OnTriggered();
        }
    }
}