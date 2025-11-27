using Data;
using GameElements.Interactable;
using Gameplay;
using Modules;
using UnityEngine;
namespace GameElements
{
/// <summary>
/// Enemy class that can move along provided path
/// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(DestructibleObject))]
    public class Enemy:MonoBehaviour,IResettableElement, ITickable
    {
        [SerializeField]
        private WaypointsSettings _Settings = null;
        [SerializeField]
        [Range(0, 1)]
        [Tooltip("Starting distance from current to next point normalized to value between 0 and 1")]
        private float _NormalizedStartingWayPosition = 0;
        [SerializeField]
        private float _MovementSpeed = 3;

        private int _CurrentWaypointIndex = 0;
        private int _Direction = 1;
        private Vector3 _MovementVector = Vector3.zero;
        private Transform _Transform = null;
        private Rigidbody _Rigidbody = null;

        private DestructibleObject _Destructible;
        private GameModule _GameModule;
        private bool _TickableRegistered = true;

        public void InitializeState(GameModule gameModule)
        {
            _GameModule = gameModule;
            if(_Destructible == null)
                _Destructible = GetComponent<DestructibleObject>();
            if (_Rigidbody == null)
                _Rigidbody = GetComponent<Rigidbody>();
            _Transform = transform;
            ResetState();
        }
        public void ResetState()
        { // Reset Enemy state to initial state
            gameObject.SetActive(true);
            _Direction = _Settings.StartingDirectionForward ? 1 : -1;
            _CurrentWaypointIndex = _Settings.StartingPosition;
            _MovementVector = _Settings.WaypointsProvider.CalculateMovementVector(_CurrentWaypointIndex, _Direction);
            var currentPosition = _Settings.WaypointsProvider.GetWaypointGlobalPosition(_CurrentWaypointIndex);
            var nextPosition = _Settings.WaypointsProvider.GetWaypointGlobalPosition(_CurrentWaypointIndex + _Direction);
            // set enemy position as linear interpolation between current and next waypoint position
            _Transform.position = Vector3.Lerp(currentPosition, nextPosition, _NormalizedStartingWayPosition);
            if (!_TickableRegistered)
            {
                _GameModule.RegisterTickableElement(this);
                _TickableRegistered = true;
            }
        }

        public void Tick(float deltaTime)
        {
            var currentPosition = _Transform.localPosition;
            var currentWaypointPosition = _Settings.WaypointsProvider[_CurrentWaypointIndex].Position;
            var targetWaypointPosition = _Settings.WaypointsProvider[_CurrentWaypointIndex + _Direction].Position;

            if ((targetWaypointPosition - currentWaypointPosition).sqrMagnitude < (currentPosition - currentWaypointPosition).sqrMagnitude)
            {
                if (_Settings.WaypointsProvider.UpdateCurrentWayPoint(ref _CurrentWaypointIndex, ref _Direction, out Vector3 movementVector))
                    _MovementVector = movementVector;
            }
            _Rigidbody.linearVelocity = _MovementVector * _MovementSpeed;
        }

        private void OnValidate()
        {
            if (_Settings.WaypointsProvider == null)
                return;
            // set enemy position when enemy parameters change
            _Settings.WaypointsProvider.ValidateStartingPositionAndDirection(ref _Settings.StartingPosition, ref _Settings.StartingDirectionForward);
            var currentPosition = _Settings.WaypointsProvider.GetWaypointGlobalPosition(_Settings.StartingPosition);
            var nextPosition = _Settings.WaypointsProvider.GetWaypointGlobalPosition(_Settings.StartingPosition + (_Settings.StartingDirectionForward ? 1 : -1));
            transform.position = Vector3.Lerp(currentPosition, nextPosition, _NormalizedStartingWayPosition);
        }

        private void OnTriggerEnter(Collider other)
        {
            var projectile = other.GetComponent<Projectile>();
            if (projectile != null)
            {
                _Destructible.NotifyDestroyed();
                gameObject.SetActive(false);
                _TickableRegistered = false;
                _GameModule.UnregisterTickableElement(this);
            }
        }
    }
}