using UnityEngine;

namespace Platformer.Scriptable.Characters.AIConfig
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/ViewFieldConfig")]
	public class ViewFieldConfig : ScriptableObject
	{
        [SerializeField]
        private Vector3 _viewOrigin;
        [SerializeField]
        private float _frontViewRange;
        [SerializeField]
        private float _behindViewRange;

        public Vector3 ViewOrigin => _viewOrigin;
        public float FrontViewRange => _frontViewRange;
        public float BehindViewRange => _behindViewRange;
    }
}