using Platformer.Scripts.LevelEnvironment.Mechanisms.Animations;
using System.Collections;
using UnityEngine;

namespace Platformer.LevelEnvironment.Mechanisms.Animations
{
    public class LeverSwitchAnimation : SimpleAnimation
    {
        [SerializeField]
        private Transform _rodAxis;
        [SerializeField]
        private Vector3 _switchedOnAxisRotation;
        [SerializeField]
        private Vector3 _switchedOffAxisRotation;

        public override float AnimationTime =>
            Vector3.Distance(_switchedOnAxisRotation, _switchedOffAxisRotation) / AnimationSpeed;

        public override void InitState(bool value) =>
            _rodAxis.rotation = Quaternion.Euler(GetTargetRotation(value));

        public override void SetSwitched(bool value) =>
            StartCoroutine(LeverSwitch(value, AnimationSpeed));

        private IEnumerator LeverSwitch(bool value, float speed)
        {
            Quaternion targetRotation = Quaternion.Euler(GetTargetRotation(value));

            Quaternion currentRotation = _rodAxis.rotation;
            while (Vector3.Distance(currentRotation.eulerAngles, targetRotation.eulerAngles) > 0.01)
            {
                yield return null;
                currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, speed * Time.deltaTime);
                _rodAxis.rotation = currentRotation;
            }
            _rodAxis.rotation = targetRotation;
        }

        private Vector3 GetTargetRotation(bool value) =>
            value ? _switchedOnAxisRotation : _switchedOffAxisRotation;
    }
}