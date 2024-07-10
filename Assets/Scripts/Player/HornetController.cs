using System;
using Player.Controls;
using Player.States;
using UnityEngine;
using Utilities.Classes;
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

        public ReactiveProperty<bool> IsGrounded { get; } = new();
        public ReactiveProperty<float> HorizontalCashedDirection { get; } = new();
        public bool IsFacingRight { get; private set; }
        public int FacingDirectionModifier { get; private set; }

        public Transform Transform => transform;
        public Rigidbody2D Rigidbody => _rigidbody;
        public Vector2 OverlapSize => _collider.size;
        public Action GroundedAction { get; set; }
        
        private Vector2 _edgeDetectionPosition => transform.position + new Vector3(_playerControllerConfig.XOffset * FacingDirectionModifier, _playerControllerConfig.YOffset);

        private readonly FiniteStateMachine _finiteStateMachine = new();
        private InputReader _inputReader;

        [Inject]
        private void Construct(InputReader inputReader)
        {
            _inputReader = inputReader;
        }

        public void SetFacingDirection(bool isFacingRight)
        {
            IsFacingRight = isFacingRight;
            FacingDirectionModifier = isFacingRight ? 1 : -1;
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
            /*_inputReader.MovePerfomedEvent += OnMove;*/
            
            _finiteStateMachine.Add(new PlayerJumpState(_finiteStateMachine, this, _inputReader, _playerControllerConfig));
            _finiteStateMachine.Add(new PlayerMoveState(_finiteStateMachine, this, _inputReader, _playerControllerConfig));
            _finiteStateMachine.Add(new PlayerFallState(_finiteStateMachine, this, _inputReader, _playerControllerConfig));
            _finiteStateMachine.Add(new PlayerClimbState(_finiteStateMachine, this, _inputReader, _playerControllerConfig));

            _finiteStateMachine.Set<PlayerFallState>();
            
            SetFacingDirection(true);
        }

        private void OnMove(Vector2 direction)
        {

        }

        private void Update()
        {
            _finiteStateMachine.Update();
        }

        private void FixedUpdate()
        {
            _finiteStateMachine.FixedUpdate();
        }

        private void OnDisable()
        {
            _finiteStateMachine.Dispose();
            /*_inputReader.MovePerfomedEvent -= OnMove;*/
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