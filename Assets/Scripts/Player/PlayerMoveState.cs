using Player.Controls;
using UnityEngine;
using Utilities.FSM;

namespace Player
{
    public class PlayerMoveState : FsmState
    {
        protected bool IsFacingRight = true;
        protected float HorizontalCashedDirection;
        
        private bool _isGrounded;
        private bool _isJumpPressed = false;
        
        private float _hangTimeCounter;
        private float _jumpBufferCounter;
        
        protected readonly InputReader InputReader;
        
        private readonly PlayerControllerConfig _playerControllerConfig;
        private readonly IPlayerController _playerController;
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _transform;
        private readonly Vector2 _overlapSize;
        
        private Vector2 _desiredVelocity;
        
        private float _overlapOffset => _playerControllerConfig.OverlapOffset;
        private float _hangTime => _playerControllerConfig.HangTime;
        private float _jumpBuffer => _playerControllerConfig.JumpBuffer;
        private float _jumpForce => _playerControllerConfig.JumpForce;
        private float _defaultLinearDrag => _playerControllerConfig.LinearDrag;
        private float _airLinearDrag => _playerControllerConfig.AirLinearDrag;
        private float _acceleration => _playerControllerConfig.MovementAcceleration;
        private float _maxSpeed => _playerControllerConfig.MoveSpeed;
        private float _lowJumpFallMultiplier => _playerControllerConfig.LowJumpFallMultiplier;
        private float _fallMultiplier => _playerControllerConfig.FallMultiplier;
        private float _fallTolerance => _playerControllerConfig.FallTolerance;
        
        private bool _isChangingDirection => (_rigidbody.velocity.x > 0f && HorizontalCashedDirection < 0f) ||
                                             (_rigidbody.velocity.x < 0f && HorizontalCashedDirection > 0f);
        
        private bool _canJump => _jumpBufferCounter > 0f && _hangTimeCounter > 0f;
        
        private LayerMask _layerMask => _playerControllerConfig.GroundLayerMask;
        
        public PlayerMoveState(FiniteStateMachine finiteStateMachine, IPlayerController playerController, InputReader inputReader, PlayerControllerConfig playerControllerConfig) : base(finiteStateMachine)
        {
            InputReader = inputReader;

            _playerControllerConfig = playerControllerConfig;
            _playerController = playerController;
            _rigidbody = playerController.Rigidbody;
            _overlapSize = playerController.OverlapSize;
            _transform = playerController.Transform;
        }

        public override void Enter()
        {
            InputReader.MovePerfomedEvent += OnMove;
            InputReader.JumpPerfomedEvent += OnJumpPerfomed;
            InputReader.JumpCancelledEvent += OnJumpCancelled;
        }

        public override void Exit()
        {
            InputReader.MovePerfomedEvent -= OnMove;
            InputReader.JumpPerfomedEvent -= OnJumpPerfomed;
            InputReader.JumpCancelledEvent -= OnJumpCancelled;
        }

        public override void FixedUpdate()
        {
            if (_isJumpPressed && HorizontalCashedDirection != 0)
            {
                if (_playerController.CanClimb())
                {
                    FiniteStateMachine.Set<PlayerClimbState>();
                }
            }

            Move();
            
            if (IsGrounded())
            {
                ApplyGroundDrag();

                _hangTimeCounter = _hangTime;
            }
            else
            {
                ApplyAirDrag();
                ApplyFallMultiplier();

                _hangTimeCounter -= Time.fixedDeltaTime;
            }

            if (_canJump)
                Jump();
        }

        public override void Update()
        {
            if (_jumpBufferCounter > 0)
                _jumpBufferCounter -= Time.deltaTime;
        }

        private bool IsGrounded()
        {
            return Physics2D.BoxCast(_transform.position, _overlapSize, 0,
                Vector2.down, _overlapOffset, _layerMask);
        }
        
        private void OnMove(Vector2 direction)
        {
            HorizontalCashedDirection = direction.x;

            if (direction.x == 0) 
                return;
            
            IsFacingRight = direction.x > 0;
            _playerController.SetFacingDirection(IsFacingRight);
        }
        
        private void OnJumpCancelled()
        {
            _isJumpPressed = false;
            _jumpBufferCounter = 0f;
        }

        private void OnJumpPerfomed()
        {
            _isJumpPressed = true;
            _jumpBufferCounter = _jumpBuffer;
        }
        
        private void ApplyFallMultiplier()
        {
            if (_rigidbody.velocity.y < _fallTolerance)
            {
                _rigidbody.gravityScale = _fallMultiplier;
            }
            else if (_rigidbody.velocity.y > _fallTolerance && !_isJumpPressed)
            {
                _rigidbody.gravityScale = _lowJumpFallMultiplier;
            }
            else
            {
                _rigidbody.gravityScale = 1f;
            }
        }

        private void ApplyAirDrag()
        {
            _rigidbody.drag = _airLinearDrag;
        }
        
        private void Jump()
        {
            ApplyAirDrag();
            _hangTimeCounter = 0f;
            _jumpBufferCounter = 0f;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
        
        private void Move()
        {
            if (Mathf.Abs(_rigidbody.velocity.x) >= _maxSpeed)
            {
                _rigidbody.velocity = new Vector2(Mathf.Sign(_rigidbody.velocity.x) * _maxSpeed, _rigidbody.velocity.y);
            }
            else
            {
                _rigidbody.AddForce(new Vector2(HorizontalCashedDirection, 0f) * _acceleration);
            }
        }

        private void ApplyGroundDrag()
        {
            if (Mathf.Abs(HorizontalCashedDirection) < .4f || _isChangingDirection)
            {
                _rigidbody.drag = _defaultLinearDrag;
            }
            else
            {
                _rigidbody.drag = 0;
            }
        }
    }
}