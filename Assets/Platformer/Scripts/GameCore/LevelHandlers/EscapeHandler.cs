using Platformer.EditorExtentions;
using Platformer.LevelEnvironment.Elements.Common;
using System;
using System.Linq;
using UnityEngine;

namespace Platformer.GameCore.LevelHandlers
{
    public class EscapeHandler : MonoBehaviour
	{
        [SerializeField, ReadOnly]
		private EscapeStageMarker[] _markers;
        [SerializeField]
        private Transform _markerContainer;
        [SerializeField]
        private PlatformMovement _hazardMovement;

		private void Start() =>
            InitMarkers();

        private void InitMarkers()
        {
            foreach (var marker in _markers)
            {
                marker.SetHandler(this);
            }
        }

		public void OnStageReached(int markerIndex) =>
            _hazardMovement.Speed = _markers[markerIndex].HazardSpeed;

#if UNITY_EDITOR
		[ContextMenu("Find and sort escape markers")]
        private void FindMarkers()
        {
            if (_markerContainer == null)
            {
                GameLogger.AddMessage("Please set marker container", GameLogger.LogType.Warning);
                return;
            }

            var markers = _markerContainer.GetComponentsInChildren<EscapeStageMarker>().ToList();

            if (!markers.Any())
            {
                GameLogger.AddMessage($"No markers found in {_markerContainer.name}", GameLogger.LogType.Warning);
                return;
            }

            for (int i = 0; i < markers.Count; i++)
            {
                int count = markers.FindAll(x => x.Index == i).Count;
                if (count > 1)
                {
                    GameLogger.AddMessage($"Index of {markers[i].name} is not unique in {_markerContainer.name}", GameLogger.LogType.Error);
                    return;
                }
            }
            markers.Sort(new Comparison<EscapeStageMarker>((item1, item2) => item1.Index.CompareTo(item2.Index)));
            _markers = markers.ToArray();
        }
#endif
	}
}