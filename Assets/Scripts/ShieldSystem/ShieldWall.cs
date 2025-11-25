using System.Linq;
using UnityEngine;
using DefaultNamespace;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using Util; // types like GridData live in DefaultNamespace

namespace ShieldSystem
{
    public class ShieldWall : MonoBehaviour
    {
        public BoxCollider LeftWallCollider;
        public BoxCollider RightWallCollider;
        
        private GridData RoomGrid => BuildingSystemManager.Instance?.PlacementSystem?.GetGridData(PlaceableType.Room);
        [SerializeField]
        private Transform _shieldCenter;

        // The left and right wall transforms (assign in inspector)
        [SerializeField]
        private Transform _leftWallParent;
        [SerializeField]
        private Transform _leftWall;
        [SerializeField]
        private Transform _rightWallParent;
        [SerializeField]
        private Transform _rightWall;

        [Tooltip("Minimum horizontal distance (in Unity units) from _shieldCenter to each wall")]
        [SerializeField]
        private float _minDistanceFromCenter = 3.5f;
        [SerializeField]
        private float _horizontalOffset = 1f;
        [SerializeField]
        private float _verticalOffset = 1f;

        [Tooltip("Size of one grid cell in Unity units")]
        [SerializeField]
        private float _cellSize = 1f;

        private void Start()
        {
            RebuildShieldWalls();   
            
            RoomGrid.OnGridDataChanged += RebuildShieldWalls;
        }
/*
#if UNITY_EDITOR
        private void OnValidate()
        {
            // Allow quick editor feedback
            if (Application.isPlaying) return;
            // Don't throw in editor if dependencies aren't ready
            if (_shieldCenter == null || _leftWallParent == null || _rightWall == null) return;
            RebuildShieldWalls();
        }
#endif*/

        /// <summary>
        /// Recomputes shield walls positions and scales based on occupied cells in RoomGrid.
        /// Left and right walls will be placed at the room edges (in world units), but never closer
        /// than _minDistanceFromCenter to _shieldCenter. Vertical scale and Y position reflect room height.
        /// </summary>
        [Button]
        public void RebuildShieldWalls()
        {
            if (RoomGrid == null || RoomGrid.PlacedInstances == null || RoomGrid.PlacedInstances.Count == 0)
            {
                // No room data - place walls at min distance from center
                _leftWallParent.transform.position = _leftWallParent.transform.position.With(x: _shieldCenter.position.x - _minDistanceFromCenter - _horizontalOffset);
                _rightWallParent.transform.position = _rightWallParent.transform.position.With(x: _shieldCenter.position.x + _minDistanceFromCenter + _horizontalOffset);
                _leftWall.localScale = _leftWall.localScale.With(y: _verticalOffset);
                _rightWall.localScale = _rightWall.localScale.With(y: _verticalOffset);
                return;
            }

            // Gather occupied cell positions
            var keys = RoomGrid.PlacedInstances.Keys.ToList();
            if (keys.Count == 0)
            {
                _leftWallParent.transform.position = _leftWallParent.transform.position.With(x: _shieldCenter.position.x - _minDistanceFromCenter - _horizontalOffset);
                _rightWallParent.transform.position = _rightWallParent.transform.position.With(x: _shieldCenter.position.x + _minDistanceFromCenter + _horizontalOffset);
                _leftWall.localScale = _leftWall.localScale.With(y: _verticalOffset);
                _rightWall.localScale = _rightWall.localScale.With(y: _verticalOffset);
                return;
            }

            int minX = keys.Min(k => k.x);
            int maxX = keys.Max(k => k.x);
            int maxY = keys.Max(k => k.y);

            // World-space edges of occupied area. We treat each cell as [_cellSize] units.
            float leftEdgeWorld = minX - _horizontalOffset;
            float rightEdgeWorld = maxX + 1 + _horizontalOffset; // right edge is one cell past the max cell index

            // Shield center x in world
            float centerX = _shieldCenter.position.x;

            // Enforce minimum distance from center
            float leftPosX = Mathf.Min(leftEdgeWorld, centerX - _minDistanceFromCenter - _horizontalOffset);
            float rightPosX = Mathf.Max(rightEdgeWorld, centerX + _minDistanceFromCenter + _horizontalOffset);

            // Vertical height
            float height = (maxY + _verticalOffset) * _cellSize;

            // Update left wall transform
            Vector3 leftPos = _leftWallParent.position;
            leftPos.x = leftPosX;
            _leftWallParent.position = leftPos;
            Vector3 leftScale = _leftWall.localScale;
            leftScale.y = Mathf.Max(0.01f, height); // avoid zero scale
            _leftWall.localScale = leftScale;

            // Update right wall transform
            Vector3 rightPos = _rightWallParent.position;
            rightPos.x = rightPosX;
            _rightWallParent.position = rightPos;
            Vector3 rightScale = _rightWall.localScale;
            rightScale.y = Mathf.Max(0.01f, height);
            _rightWall.localScale = rightScale;
        }
    }
}