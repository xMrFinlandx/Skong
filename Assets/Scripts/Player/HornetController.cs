using Player.Controls;
using UnityEngine;
using Utilities.FSM;
using Zenject;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class HornetController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private PlayerControllerConfig _playerControllerConfig;
        [Space]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private BoxCollider2D _collider;

        public Transform Transform => transform;
        public Rigidbody2D Rigidbody => _rigidbody;
        public Vector2 OverlapSize => _collider.size;

        private Vector2 _edgeDetectionPosition => transform.position + new Vector3(_playerControllerConfig.XOffset * _facingDirectionModifier, _playerControllerConfig.YOffset);
        
        private bool _isFacingRight;
        private int _facingDirectionModifier;
        
        private readonly FiniteStateMachine _finiteStateMachine = new();
        private InputReader _inputReader;

        [Inject]
        private void Construct(InputReader inputReader)
        {
            _inputReader = inputReader;
        }

        public void SetFacingDirection(bool isFacingRight)
        {
            _isFacingRight = isFacingRight;
            _facingDirectionModifier = isFacingRight ? 1 : -1;
        }
        
        public bool CanClimb()
        {
            return Physics2D.Raycast(_edgeDetectionPosition, Vector2.down, 
                       _playerControllerConfig.GroundRayLength, _playerControllerConfig.GroundLayerMask) && 
                   !Physics2D.Raycast(_edgeDetectionPosition, Vector2.up, 
                       _playerControllerConfig.AirRayLength, _playerControllerConfig.GroundLayerMask);
        }
        
        private void OnValidate()
        {
            _rigidbody ??= GetComponent<Rigidbody2D>();
            _collider ??= GetComponent<BoxCollider2D>();

            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        private void Start()
        {
            _finiteStateMachine.Add(new PlayerMoveState(_finiteStateMachine, this, _inputReader, _playerControllerConfig));
            _finiteStateMachine.Add(new PlayerClimbState(_finiteStateMachine, this, _playerControllerConfig));
            _finiteStateMachine.Set<PlayerMoveState>();
            
            SetFacingDirection(true);
        }

        private void Update()
        {
            _finiteStateMachine.Update();
        }

        private void FixedUpdate()
        {
            _finiteStateMachine.FixedUpdate();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position - new Vector3(0, _playerControllerConfig.OverlapOffset), OverlapSize);
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(_edgeDetectionPosition, Vector3.down * _playerControllerConfig.GroundRayLength);
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_edgeDetectionPosition, Vector3.up * _playerControllerConfig.AirRayLength);
        }
    }
}