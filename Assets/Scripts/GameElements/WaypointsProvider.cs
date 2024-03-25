using Data;
using Gameplay;
using UnityEngine;
using Utils;
namespace GameElements
{
    /// <summary>
    /// Class responsible for providing functionality of way points
    /// </summary>
    public class WaypointsProvider:MonoBehaviour, IGizmoDrawable
    {
        [SerializeField]
        private WayPoint[] _WayPoints = null;
        [SerializeField]
        private GizmoSettings _GizmoSettings = null;
        public WayPoint this[int index] => _WayPoints[index];
        public int WaypointsCount => _WayPoints.Length;
        public Color TargetGizmoColor => _GizmoSettings.TargetGizmoColor;
        public TargetShapes TargetGizmoShape => _GizmoSettings.TargetGizmoShape;
        public Vector3 TargetGizmoSize => _GizmoSettings.TargetGizmoSize;
        public Vector3 TargetGizmoOffset => _GizmoSettings.TargetGizmoOffset;

        /// <summary>
        /// Global position of a way point
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetWaypointGlobalPosition(int index) => transform.position + _WayPoints[index].Position;
        /// <summary>
        /// Calculate normalized vector from current point to next point
        /// </summary>
        /// <param name="currentPosition"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Vector3 CalculateMovementVector(int currentPosition, int direction)
        {
            if (_WayPoints == null || _WayPoints.Length < 2)
                return Vector3.zero;
            return (_WayPoints[currentPosition + direction].Position - _WayPoints[currentPosition].Position).normalized;
        }
        /// <summary>
        /// Update current way point
        /// </summary>
        /// <param name="currentWaypointIndex"></param>
        /// <param name="direction"></param>
        /// <param name="movementVector"></param>
        /// <returns></returns>
        public bool UpdateCurrentWayPoint(ref int currentWaypointIndex, ref int direction, out Vector3 movementVector)
        {
            currentWaypointIndex += direction;
            movementVector = Vector3.zero;

            if (direction > 0 && currentWaypointIndex == _WayPoints.Length - 1 || direction < 0 && currentWaypointIndex == 0)
            {
                direction *= -1;
                movementVector = CalculateMovementVector(currentWaypointIndex, direction);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validate set position and direction. Set to valid if provided values are invalid.
        /// </summary>
        /// <param name="startingPosition"></param>
        /// <param name="startingDirection"></param>
        public void ValidateStartingPositionAndDirection(ref int startingPosition, ref bool startingDirection)
        {
            if (WaypointsCount < 2)
                return;
            if (startingPosition >=  WaypointsCount)
            {
                startingPosition =  WaypointsCount - 1;
                startingDirection = false;
            }
            else if (startingPosition < 0)
            {
                startingPosition = 0;
                startingDirection = true;
            }
        }

        /// <summary>
        /// Draw waypoints path for easier level design
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if(_WayPoints == null)
                return;
            var position = transform.position;
            for (var i = 0; i < _WayPoints.Length; i++)
            {
                var currentPosition = _WayPoints[i].Position;
                GizmoDrawer.DrawGizmo(position + currentPosition, this);
                if (i < _WayPoints.Length - 1)
                {
                    var nextPosition = _WayPoints[i + 1].Position;
                    Gizmos.color = _GizmoSettings.LineColor;
                    Gizmos.DrawLine(position + currentPosition, position + nextPosition);
                }
            }
        }
    }
}