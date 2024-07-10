using Player.Controls;
using UnityEngine;
using Utilities.FSM;

namespace Player.States
{
    public class PlayerJumpState : PlayerMoveState
    {
        private bool _isGrounded;
        private bool _isJumpPressed = false;
        private bool _isJumping;
        
        private float _hangTime => PlayerControllerConfig.HangTime;
        private float _jumpBuffer => PlayerControllerConfig.JumpBuffer;
        private float _jumpForce => PlayerControllerConfig.JumpForce;
        private float _airLinearDrag => PlayerControllerConfig.AirLinearDrag;
        private float _lowJumpFallMultiplier => PlayerControllerConfig.LowJumpFallMultiplier;
        private float _fallMultiplier => PlayerControllerConfig.FallMultiplier;
        private float _fallTolerance => PlayerControllerConfig.FallTolerance;

        public PlayerJumpState(FiniteStateMachine finiteStateMachine, IPlayerController playerController,
            InputReader inputReader, PlayerControllerConfig playerControllerConfig) : base(finiteStateMachine,
            playerController, inputReader, playerControllerConfig)
        {
        }

        public override void Enter()
        {
            InputReader.MovePerfomedEvent += OnMove;
            InputReader.JumpCancelledEvent += OnJumpCancelled;

            _isJumpPressed = true;
        }

        public override void Exit()
        {
            InputReader.MovePerfomedEvent -= OnMove;
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

            _isGrounded = IsGrounded();
            PlayerController.IsGrounded.Value = _isGrounded;
            
            ApplyFallMultiplier();

            if (!_isJumping)
                Jump();
        }
        
        private void OnJumpCancelled()
        {
            _isJumpPressed = false;
            _isJumping = false;
            
            FiniteStateMachine.Set<PlayerFallState>();
        }

        private void ApplyFallMultiplier()
        { 
            if (Rigidbody.velocity.y < _fallTolerance)
            {
                Rigidbody.gravityScale = _fallMultiplier;
            }
            else if (Rigidbody.velocity.y > _fallTolerance && !_isJumpPressed)
            {
                Rigidbody.gravityScale = _lowJumpFallMultiplier;
                FiniteStateMachine.Set<PlayerFallState>();
            }
            else
            {
                Rigidbody.gravityScale = 1f;
            }
        }

        private void ApplyAirDrag()
        {
            Rigidbody.drag = _airLinearDrag;
        }

        private void Jump()
        {
            ApplyAirDrag();
            _isJumping = true;
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0f);
            Rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }
}