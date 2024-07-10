using UnityEngine;
using Utilities.FSM;

namespace Player.States
{
    public class PlayerClimbState : FsmState
    {
        private readonly PlayerControllerConfig _playerControllerConfig;
        private readonly Rigidbody2D _rigidbody;
        
        public PlayerClimbState(FiniteStateMachine finiteStateMachine, IPlayerController playerController, PlayerControllerConfig playerControllerConfig) : base(finiteStateMachine)
        {
            _playerControllerConfig = playerControllerConfig;
            _rigidbody = playerController.Rigidbody;
        }

        public override void Enter()
        {
            Climb();
        }

        private void Climb()
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(new Vector2(0, _playerControllerConfig.ClimbJumpForce), ForceMode2D.Impulse);
            FiniteStateMachine.Set<PlayerMoveState>();
        }
    }
}