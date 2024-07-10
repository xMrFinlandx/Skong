using Player.Controls;
using UnityEngine;
using Utilities.FSM;

namespace Player.States
{
    public class PlayerFallState : PlayerMoveState
    {
        private float _jumpBufferCounter;
        private float _hangTimeCounter;
        
        private bool _isJumpPressed;
        private bool _isGrounded;
        
        private float _hangTime => PlayerControllerConfig.HangTime;
        private float _jumpBuffer => PlayerControllerConfig.JumpBuffer;
        private float _lowJumpFallMultiplier => PlayerControllerConfig.LowJumpFallMultiplier;
        private float _fallMultiplier => PlayerControllerConfig.FallMultiplier;
        private float _fallTolerance => PlayerControllerConfig.FallTolerance;
        
        private bool _canJump => (_jumpBufferCounter > 0f && _hangTimeCounter > 0f) || (_jumpBufferCounter > 0 && _isGrounded);
        
        public PlayerFallState(FiniteStateMachine finiteStateMachine, IPlayerController playerController, InputReader inputReader, PlayerControllerConfig playerControllerConfig) : 
            base(finiteStateMachine, playerController, inputReader, playerControllerConfig)
        {
        }

        public override void Enter()
        {
            if (FiniteStateMachine.PreviousState?.GetType() != typeof(PlayerJumpState))
            {
                _hangTimeCounter = _hangTime;
            }
            
            InputReader.MovePerfomedEvent += OnMove;
            InputReader.JumpPerfomedEvent += OnJumpPerfomed;
            InputReader.JumpCancelledEvent += OnJumpCancelled;
        }

        private void OnJumpCancelled()
        {
            _isJumpPressed = false;
            _hangTimeCounter = 0;
            _jumpBufferCounter = 0;
        }

        private void OnJumpPerfomed()
        {
            Debug.Log( "jump perfomed");
            
            _isJumpPressed = true;
            _jumpBufferCounter = _jumpBuffer;
        }

        public override void Exit()
        {
            _hangTimeCounter = 0;
            _jumpBufferCounter = 0;
            _isJumpPressed = false;
            
            InputReader.MovePerfomedEvent -= OnMove;
            InputReader.JumpPerfomedEvent -= OnJumpPerfomed;
            InputReader.JumpCancelledEvent -= OnJumpCancelled;
        }

        public override void FixedUpdate()
        {
            if (_isJumpPressed && HorizontalCashedDirection != 0)
            {
                if (PlayerController.CanClimb())
                {
                    FiniteStateMachine.Set<PlayerClimbState>();
                }
            }
            
            Move();
            ApplyFallMultiplier();

            _isGrounded = IsGrounded();
            
            if (_canJump)
            {
                FiniteStateMachine.Set<PlayerJumpState>();
            }
            
            else if (_isGrounded)
            {
                FiniteStateMachine.Set<PlayerMoveState>();
            }
        }

        private void ApplyFallMultiplier()
        {
            if (Rigidbody.velocity.y < _fallTolerance)
            {
                Rigidbody.gravityScale = _fallMultiplier;
            }
            else if (Rigidbody.velocity.y > _fallTolerance)
            {
                Rigidbody.gravityScale = _lowJumpFallMultiplier;
            }
        }

        public override void Update()
        {
            if (_jumpBufferCounter > 0)
                _jumpBufferCounter -= Time.deltaTime;
        }
    }
}