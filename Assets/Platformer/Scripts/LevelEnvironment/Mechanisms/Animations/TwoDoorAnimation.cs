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
            StartCoroutine(DoorOpen(_leftDoorAxis, -GetTargetRotation(value)));
            StartCoroutine(DoorOpen(_rightDoorAxis, GetTargetRotation(value)));
        }

        private IEnumerator DoorOpen(Transform doorAxis, Vector3 targetRotation)
        {
            Vector3 currentRotation = doorAxis.rotation.eulerAngles;

            while (Vector3.Distance(currentRotation, targetRotation) > 0.01)
            {
                yield return null;
                currentRotation.y += AnimationSpeed * Time.deltaTime;
                //currentRotation.y += 
                doorAxis.localRotation = Quaternion.Euler(currentRotation);

                //currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, AnimationSpeed * Time.deltaTime);
                //doorAxis.localRotation = currentRotation;
            }
            doorAxis.localRotation = Quaternion.Euler(targetRotation);
        }

        private Vector3 GetTargetRotation(bool value) =>
            value ? _openDoorAxisRotation : _closedDoorAxisRotation;
    }
}
