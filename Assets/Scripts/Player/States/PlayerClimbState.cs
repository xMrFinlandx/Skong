using Player.Controls;
using UnityEngine;
using Utilities.FSM;

namespace Player.States
{
    public class PlayerClimbState : PlayerMoveState
    {
        private readonly PlayerControllerConfig _playerControllerConfig;
        private readonly Rigidbody2D _rigidbody;
        private IPlayerController _playerController;

        private float _direction;

        public PlayerClimbState(FiniteStateMachine finiteStateMachine, IPlayerController playerController,
            InputReader inputReader, PlayerControllerConfig playerControllerConfig) : base(finiteStateMachine,
            playerController, inputReader, playerControllerConfig)
        {
            _playerControllerConfig = playerControllerConfig;
            _playerController = playerController;
            _rigidbody = playerController.Rigidbody;
        }

        public override void Enter()
        {
            Climb();
        }
        
        private void Climb()
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.gravityScale = 1;
            _rigidbody.AddForce(new Vector2(_playerController.HorizontalCashedDirection.Value, _playerControllerConfig.ClimbJumpForce), ForceMode2D.Impulse);
            FiniteStateMachine.Set<PlayerMoveState>();
        }
    }
}