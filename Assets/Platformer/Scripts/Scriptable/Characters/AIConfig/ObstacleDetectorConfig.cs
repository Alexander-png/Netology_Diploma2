using UnityEngine;

namespace Platformer.Scriptable.Characters.AIConfig
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/ObstacleDetectorConfig")]
	public class ObstacleDetectorConfig : ScriptableObject
	{
        [SerializeField]
        private Vector3 _horizontalSensorOrigin;
        [SerializeField]
        private float _horizontalSensorLength;
        [SerializeField]
        private float _verticalSensorLength;
        [SerializeField]
        private float _verticalSensorOffset;

        public Vector3 HorizontalSensorOrigin => _horizontalSensorOrigin;
        public float HorizontalSensorLength => _horizontalSensorLength;
        public float VerticalSensorLength => _verticalSensorLength;
        public float VerticalSensorOffset => _verticalSensorOffset;
    }
}