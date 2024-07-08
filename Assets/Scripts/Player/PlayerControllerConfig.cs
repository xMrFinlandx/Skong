using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Player Controller Config", menuName = "Player/Player Controller Config", order = 0)]
    public class PlayerControllerConfig : ScriptableObject
    {
        [SerializeField] private float _movementAcceleration = 50f;
        [SerializeField] private float _linearDrag = 10f;
        [SerializeField] private float _moveSpeed = 12f;
        [Space] 
        [SerializeField] private float _fallTolerance = .5f;
        [SerializeField] private float _hangTime = .15f;
        [SerializeField] private float _jumpBuffer = .15f;
        [SerializeField] private float _jumpForce = 20f;
        [SerializeField] private float _airLinearDrag = 2.5f;
        [SerializeField] private float _fallMultiplier = 8f;
        [SerializeField] private float _lowJumpFallMultiplier = 5f;
        [Header("Ground detection")]
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private float _overlapOffset = .05f;
        [Header("Edge detection")] 
        [SerializeField] private float _xOffset;
        [SerializeField] private float _yOffset;
        [SerializeField] private float _groundRayLength = .3f;
        [SerializeField] private float _airRayLength = .1f;
        [Header("Climbing")] 
        [SerializeField] private float _climbJumpForce = 10;
        
        public LayerMask GroundLayerMask => _groundLayerMask;
        
        public float MovementAcceleration => _movementAcceleration;
        public float LinearDrag => _linearDrag;
        public float MoveSpeed => _moveSpeed;
        public float JumpForce => _jumpForce;
        public float AirLinearDrag => _airLinearDrag;
        public float OverlapOffset => _overlapOffset;
        public float LowJumpFallMultiplier => _lowJumpFallMultiplier;
        public float FallMultiplier => _fallMultiplier;
        public float HangTime => _hangTime;
        public float JumpBuffer => _jumpBuffer;
        public float FallTolerance => _fallTolerance;
        public float XOffset => _xOffset;
        public float YOffset => _yOffset;
        public float GroundRayLength => _groundRayLength;
        public float AirRayLength => _airRayLength;
        public float ClimbJumpForce => _climbJumpForce;
    }
}