using Player.Controls;
using UnityEngine;
using Utilities.FSM;

namespace Player.States
{
    public class PlayerMoveState : PlayerBaseMoveState
    {
        private bool _isGrounded;
        
        private float _jumpBuffer => PlayerControllerConfig.JumpBuffer;
        private float _hangTime => PlayerControllerConfig.HangTime;
        private float _defaultLinearDrag => PlayerControllerConfig.LinearDrag;
        private float _acceleration => PlayerControllerConfig.MovementAcceleration;
        private float _maxSpeed => PlayerControllerConfig.MoveSpeed;
        
        private bool _isChangingDirection => (Rigidbody.velocity.x > 0f && HorizontalCashedDirection < 0f) ||
                                             (Rigidbody.velocity.x < 0f && HorizontalCashedDirection > 0f);
        

        public PlayerMoveState(FiniteStateMachine finiteStateMachine, IPlayerController playerController,
            InputReader inputReader, PlayerControllerConfig playerControllerConfig) : base(finiteStateMachine,
            playerController, inputReader, playerControllerConfig)
        {
        }

        public override void Enter()
        {
            InputReader.JumpPerfomedEvent += OnJumpPerfomed;
            InputReader.MovePerfomedEvent += OnMove;
            
            ApplyGroundDrag();
        }

        public override void Exit()
        {
            InputReader.JumpPerfomedEvent -= OnJumpPerfomed;
        }

        public override void FixedUpdate()
        {
            Move();

            _isGrounded = IsGrounded();
            PlayerController.IsGrounded.Value = _isGrounded;
            
            if (_isGrounded)
            {
                ApplyGroundDrag();
            }
            else
            {
                FiniteStateMachine.Set<PlayerFallState>();
            }
        }
        
        protected override void OnMove(Vector2 direction)
        {
            HorizontalCashedDirection = direction.x;
            
            if (direction.x == 0) 
                return;
            
            IsFacingRight = direction.x > 0;
            PlayerController.SetFacingDirection(IsFacingRight);
        }
        
        protected override void Move()
        {
            if (Mathf.Abs(Rigidbody.velocity.x) >= _maxSpeed)
            {
                Rigidbody.velocity = new Vector2(Mathf.Sign(Rigidbody.velocity.x) * _maxSpeed, Rigidbody.velocity.y);
            }
            else
            {
                Rigidbody.AddForce(new Vector2(HorizontalCashedDirection, 0f) * _acceleration);
            }
        }

        private void ApplyGroundDrag()
        {
            if (Mathf.Abs(HorizontalCashedDirection) < .4f || _isChangingDirection)
            {
                Rigidbody.drag = _defaultLinearDrag;
            }
            else
            {
                Rigidbody.drag = 0;
            }
        }
        
        private void OnJumpPerfomed()
        {
            FiniteStateMachine.Set<PlayerJumpState>();
        }
    }
}