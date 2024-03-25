using UnityEngine;
namespace Data.Settings
{
    [CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Settings/Player Movement")]
    public class PlayerMovementSettings:ScriptableObject
    {
        [SerializeField]
        private float _Speed = 5;
        [SerializeField]
        private float _JumpForce = 5;
        [SerializeField]
        private float _MaxJumpsAllowed = 2;
        [SerializeField]
        private Vector3 _MovementDirection = Vector3.forward;

        public float Speed => _Speed;
        public float JumpForce => _JumpForce;
        public float MaxJumpsAllowed => _MaxJumpsAllowed;
        public Vector3 MovementDirection => _MovementDirection;

    }
}