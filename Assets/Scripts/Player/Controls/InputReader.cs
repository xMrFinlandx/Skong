using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controls
{
    [CreateAssetMenu(fileName = "New Input Reader", menuName = "Input Reader", order = 0)]
    public class InputReader : ScriptableObject, GameControls.IGameplayActions, GameControls.IUIActions
    {
        private GameControls _gameControls;

        public event Action<Vector2> MovePerfomedEvent;
        public event Action MoveCancelledEvent;

        public event Action JumpPerfomedEvent;
        public event Action JumpCancelledEvent;

        public event Action AttackPerfomedEvent;
        public event Action DashPerfomedEvent;

        public event Action HealPerfomedEvent;
        
        public event Action PausePerfomedEvent;
        public event Action ResumePerfomedEvent;

        public void Disable()
        {
            _gameControls.UI.Disable();
            _gameControls.Gameplay.Disable();
        }

        public void SetUI()
        {
            _gameControls.UI.Enable();
            _gameControls.Gameplay.Disable();
        }

        public void SetGameplay()
        {
            _gameControls.Gameplay.Enable();
            _gameControls.UI.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovePerfomedEvent?.Invoke(context.ReadValue<Vector2>());

            if (context.phase == InputActionPhase.Canceled)
                MoveCancelledEvent?.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                JumpPerfomedEvent?.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                JumpCancelledEvent?.Invoke();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                AttackPerfomedEvent?.Invoke();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                DashPerfomedEvent?.Invoke();
        }

        public void OnHeal(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                HealPerfomedEvent?.Invoke();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                PausePerfomedEvent?.Invoke();
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                ResumePerfomedEvent?.Invoke();
        }

        private void OnEnable()
        {
            if (_gameControls != null)
                return;

            _gameControls = new GameControls();
            
            _gameControls.Gameplay.SetCallbacks(this);
            _gameControls.UI.SetCallbacks(this);
            
            SetGameplay();
        }
    }
}