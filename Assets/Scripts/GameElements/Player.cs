using Data.Settings;
using Modules;
using UnityEngine;
namespace GameElements
{
    public class Player:MonoBehaviour,IPlayer
    {
        [SerializeField]
        private Weapon _Weapon = null;
        [SerializeField]
        private PlayerMovementSettings _PlayerMovementSettings = null;
        [SerializeField]
        private Rigidbody _Rigidbody = null;
        [SerializeField]
        private Vector3 _GroundCheckOffset = Vector3.down;
        [SerializeField]
        private Vector3 _GroundCheckSize = Vector3.one;
        [SerializeField]
        private Vector3 _WallCheckOffset = Vector3.zero;
        [SerializeField]
        private Vector3 _WallCheckSize = Vector3.one;

        [SerializeField]
        private LayerMask _GroundLayerMask;

        [SerializeField]
        private Transform _DefaultParent = null;

        private Vector3 _StartPosition = Vector3.zero;
        private Quaternion _StartRotation = Quaternion.identity;
        private int _JumpsCount = 0;
        private bool _IsOnGround = false;
        private Transform _Transform = null;

        public void InitializeState(GameModule gameModule)
        {
            _Transform = transform;
            _Weapon.Initialize(gameModule);
            var playerTransform = transform;
            _StartPosition = playerTransform.position;
            _StartRotation = playerTransform.rotation;
        }

        public void ResetState()
        {
            _Weapon.ResetWeapon();
            var playerTransform = transform;
            playerTransform.SetPositionAndRotation(_StartPosition, _StartRotation);
        }

        public void Move(float dir)
        {
            if(!Mathf.Approximately(dir, 0))
                transform.rotation = Quaternion.LookRotation(_PlayerMovementSettings.MovementDirection * dir, Vector3.up);

            if (!_IsOnGround && CheckTouchBox(_WallCheckOffset, _WallCheckSize))
                dir = 0; // not allow to move in direction of wall when wall is touched to prevent sticking
            var vector = _PlayerMovementSettings.MovementDirection * (dir * _PlayerMovementSettings.Speed);
            _Rigidbody.velocity = new Vector3(vector.x, _Rigidbody.velocity.y, vector.z);
        }

        public void Jump()
        {
            if(_JumpsCount >= _PlayerMovementSettings.MaxJumpsAllowed)
                return;
            _JumpsCount++;
            _Rigidbody.AddForce(Vector3.up * _PlayerMovementSettings.JumpForce, ForceMode.Impulse);

        }
        public void Fire()
        {
            if(_Weapon != null)
                _Weapon.Fire();
        }

        public void Tick(float deltaTime)
        {
            _IsOnGround = CheckTouchBox(_GroundCheckOffset, _GroundCheckSize);
            if (_IsOnGround)
                _JumpsCount = 0;
            else
                _JumpsCount = Mathf.Max(_JumpsCount, 1);
        }

        private void OnCollisionEnter(Collision other)
        {
            if(!IsOnGroundLayer(other.gameObject.layer))
                return;
            transform.parent = other.transform;

        }

        private void OnCollisionExit(Collision other)
        {
            if(!IsOnGroundLayer(other.gameObject.layer))
                return;
            transform.parent = _DefaultParent;
        }

        private bool IsOnGroundLayer(int layer) => (1 << layer & _GroundLayerMask) != 0;


        private bool CheckTouchBox(Vector3 offset, Vector3 size)
        {
            var rotation = _Transform.rotation;
            return Physics.CheckBox(_Transform.position + rotation * offset, size, rotation, _GroundLayerMask);
        }

        /// <summary>
        /// Draw gizmos for ground check and wall check for easier positioning in editor
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            var rotation = transform.rotation;
            Gizmos.DrawWireCube(position + rotation * _GroundCheckOffset, _GroundCheckSize);
            Gizmos.DrawWireCube(position + rotation * _WallCheckOffset, _WallCheckSize);
        }

    }
}