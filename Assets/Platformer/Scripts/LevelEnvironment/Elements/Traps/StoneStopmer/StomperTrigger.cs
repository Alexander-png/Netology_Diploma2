using UnityEngine;

namespace Platformer.LevelEnvironment.Elements.Traps.StoneStomper
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