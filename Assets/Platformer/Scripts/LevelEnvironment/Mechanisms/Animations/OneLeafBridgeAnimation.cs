using Platformer.Scripts.LevelEnvironment.Mechanisms.Animations;
using System.Collections;
using UnityEngine;

namespace Platformer.LevelEnvironment.Mechanisms.Animations
{
    public class OneLeafBridgeAnimation : SimpleAnimation
    {
        [SerializeField]
        private Transform _leafAxis;

        [SerializeField]
        private Vector3 _raisedBridgeRotation;
        [SerializeField]
        private Vector3 _loweredBridgeRotation;

        public override float AnimationTime =>
            Vector3.Distance(_raisedBridgeRotation, _loweredBridgeRotation) / AnimationSpeed;

        public override void InitState(bool value) =>
            _leafAxis.rotation = Quaternion.Euler(GetTargetRotation(value));

        public override void SetSwitched(bool value)
        {
            StartCoroutine(ChangeBridgeState(Quaternion.Euler(GetTargetRotation(value))));;
        }

        private IEnumerator ChangeBridgeState(Quaternion targetRotation)
        {
            Quaternion currentRotation = _leafAxis.rotation;
            while (Vector3.Distance(currentRotation.eulerAngles, targetRotation.eulerAngles) > 0.01)
            {
                yield return null;
                currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, AnimationSpeed * Time.deltaTime);
                _leafAxis.rotation = currentRotation;
            }
            _leafAxis.rotation = targetRotation;
        }

        private Vector3 GetTargetRotation(bool value) =>
            value ? _loweredBridgeRotation : _raisedBridgeRotation;
    }
}