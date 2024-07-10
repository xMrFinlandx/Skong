using Player.Controls;
using UnityEngine;
using Utilities.FSM;

namespace Player.States
{
    public abstract class PlayerBaseMoveState : FsmState
    {
        protected bool IsFacingRight;
        
        protected readonly InputReader InputReader;
        protected readonly IPlayerController PlayerController;
        protected readonly Rigidbody2D Rigidbody;
        protected readonly Transform Transform;
        protected readonly PlayerControllerConfig PlayerControllerConfig;
        
        private readonly Vector2 _overlapSize;
        private float _overlapOffset => PlayerControllerConfig.OverlapOffset;
        private LayerMask _layerMask => PlayerControllerConfig.GroundLayerMask;
        
        protected float HorizontalCashedDirection
        {
            get => PlayerController.HorizontalCashedDirection.Value;
            set => PlayerController.HorizontalCashedDirection.Value = value;
        }

        protected PlayerBaseMoveState(FiniteStateMachine finiteStateMachine, IPlayerController playerController, InputReader inputReader, PlayerControllerConfig playerControllerConfig) : base(finiteStateMachine)
        {
            InputReader = inputReader;
            PlayerController = playerController;
            Rigidbody = playerController.Rigidbody;
            Transform = playerController.Transform;
            PlayerControllerConfig = playerControllerConfig;

            _overlapSize = PlayerController.OverlapSize;
        }

        protected abstract void OnMove(Vector2 direction);
        protected abstract void Move();

        protected bool IsGrounded()
        {
            return Physics2D.BoxCast(Transform.position, _overlapSize, 0,
                Vector2.down, _overlapOffset, _layerMask);
        }
    }
}