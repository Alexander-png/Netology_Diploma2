using Platformer.Scripts.LevelEnvironment.Mechanisms.Animations;
using System.Collections;
using UnityEngine;

namespace Platformer.LevelEnvironment.Mechanisms.Animations
{
    public class TwoDoorAnimation : SimpleAnimation
    {
        [SerializeField]
        private Transform _leftDoorAxis;
        [SerializeField]
        private Transform _rightDoorAxis;

        [SerializeField]
        private Vector3 _openDoorAxisRotation;
        [SerializeField]
        private Vector3 _closedDoorAxisRotation;

        public override float AnimationTime => 
            Vector3.Distance(_closedDoorAxisRotation, _openDoorAxisRotation) / AnimationSpeed;

        public override void InitState(bool value)
        {
            _leftDoorAxis.localRotation = Quaternion.Euler(-GetTargetRotation(value));
            _rightDoorAxis.localRotation = Quaternion.Euler(GetTargetRotation(value));
        }

        public override void SetSwitched(bool value)
        {
            StartCoroutine(DoorOpen(_leftDoorAxis, Quaternion.Euler(-GetTargetRotation(value))));
            StartCoroutine(DoorOpen(_rightDoorAxis, Quaternion.Euler(GetTargetRotation(value))));
        }

        private IEnumerator DoorOpen(Transform doorAxis, Quaternion targetRotation)
        {
            Quaternion currentRotation = doorAxis.localRotation;
            while (Vector3.Distance(currentRotation.eulerAngles, targetRotation.eulerAngles) > 0.01)
            {
                yield return null;
                currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, AnimationSpeed * Time.deltaTime);
                doorAxis.localRotation = currentRotation;
            }
            doorAxis.localRotation = targetRotation;
        }

        private Vector3 GetTargetRotation(bool value) =>
            value ? _openDoorAxisRotation : _closedDoorAxisRotation;
    }
}
