using Platformer.CharacterSystem.Base;
using Platformer.EditorExtentions;
using Platformer.LevelEnvironment.Switchers;
using UnityEngine;

namespace Platformer.LevelEnvironment.Mechanisms.Switchers
{
    public class EnityDeathHandler : Switch
    {
        [SerializeField]
        private Entity _observableEntity;

        private IDamagable _damagableInterface;
        private ISwitchTarget _switchTarget;

        public override bool CanPerform => true;
        public override bool IsSwitchedOn 
        { 
            get => _switchTarget.IsSwitchedOn;
            set 
            {
                if (_switchTarget != null)
                {
                    _switchTarget.IsSwitchedOn = value;
                }
            }
        }

        private void OnEnable()
        {
            if (_observableEntity == null)
            {
                return;
            }
            _damagableInterface = _observableEntity as IDamagable;
            _damagableInterface.Died += OnEntityDied;
        }

        private void OnDisable()
        {
            if (_damagableInterface == null)
            {
                return;
            }
            _damagableInterface.Died -= OnEntityDied;
        }

        private void OnEntityDied(object sender, System.EventArgs e) =>
            IsSwitchedOn = !IsSwitchedOn;

        protected override void Start()
        {
            base.Start();
            if (Target == null)
            {
                GameLogger.AddMessage($"{gameObject.name}: target not specified.", GameLogger.LogType.Warning);
                return;
            }

            _switchTarget = Target.GetComponent<ISwitchTarget>();
            if (_switchTarget == null)
            {
                GameLogger.AddMessage($"{gameObject.name}: the {Target.name} does not contain ISwitcherTarget component.", GameLogger.LogType.Error);
                return;
            }

            _switchTarget.InitState(IsSwitchedOn);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_observableEntity != null)
            {
                if ((_observableEntity as IDamagable) == null)
                {
                    GameLogger.AddMessage($"{_observableEntity} is not damagable. Can not handle this.");
                    _observableEntity = null;
                }
            }
        }
#endif
    }
}