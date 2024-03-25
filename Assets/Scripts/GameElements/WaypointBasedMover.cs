using Data;
using Modules;
using UnityEngine;

namespace GameElements
{
    /// <summary>
    /// Moves target transform along provided waypoints
    /// </summary>
    public class WaypointBasedMover : MonoBehaviour, IResettableElement,ITickable
    {
        [SerializeField]
        private WaypointsSettings _Settings = null;

        [SerializeField]
        private float _MovementSpeed = 1;
        [SerializeField]
        private Transform _Target = null;

        private int _CurrentWaypointIndex = 0;
        private int _Direction = 1;
        private Vector3 _MovementVector = Vector3.zero;
        private float _WaitTime = 0;

        public void InitializeState(GameModule gameModule)
        {
            ResetState();
        }
        public void ResetState()
        {
            _Direction = _Settings.StartingDirectionForward ? 1 : -1;
            _CurrentWaypointIndex = _Settings.StartingPosition;
            var waypoint = _Settings.WaypointsProvider[_CurrentWaypointIndex];
            _Target.localPosition = waypoint.Position;
            _WaitTime = waypoint.WaitTime;
            _MovementVector = _Settings.WaypointsProvider.CalculateMovementVector(_CurrentWaypointIndex, _Direction);
        }

        public void Tick(float deltaTime)
        {
            _WaitTime -= deltaTime;
            if (_WaitTime > 0)
                return;
            var additionalTime = -_WaitTime;
            _WaitTime = 0;
            var currentLiftPosition = _Target.localPosition;
            var targetPosition = _Settings.WaypointsProvider[_CurrentWaypointIndex + _Direction].Position;
            var newPosition = currentLiftPosition + _MovementVector * (_MovementSpeed * (deltaTime + additionalTime));
            if ((targetPosition - currentLiftPosition).sqrMagnitude < (newPosition - currentLiftPosition).sqrMagnitude)
            {
                _Target.localPosition = targetPosition;
                if (_Settings.WaypointsProvider.UpdateCurrentWayPoint(ref _CurrentWaypointIndex, ref _Direction, out Vector3 movementVector))
                    _MovementVector = movementVector;

                _WaitTime = _Settings.WaypointsProvider[_CurrentWaypointIndex].WaitTime;
            }
            else
                _Target.localPosition = newPosition;

        }
        private void OnValidate()
        {
            if (_Settings.WaypointsProvider == null)
                return;
            _Settings.WaypointsProvider.ValidateStartingPositionAndDirection(ref _Settings.StartingPosition, ref _Settings.StartingDirectionForward);
        }

    }

}