using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.FSM
{
    public class FiniteStateMachine
    {
        public FsmState CurrentState { get; private set; }
        public FsmState PreviousState { get; private set; }

        private readonly Dictionary<Type, FsmState> _states = new();

        private FsmState _awaitedState;
        private bool _stateLocked;
        
        private float _lockTimeCounter;

        public bool IsEqualsCurrentState<T>() where T : FsmState
        {
            return CurrentState.GetType() == typeof(T);
        }

        public void Add(FsmState state)
        {
            _states.Add(state.GetType(), state);
        }

        public void LockState(float time)
        {
            _lockTimeCounter = time;
            _stateLocked = true;
        }

        public void Set<T>() where T : FsmState
        {
            var type = typeof(T);
            
            if (CurrentState != null && IsEqualsCurrentState<T>())
                return;
            
            if (!_states.TryGetValue(type, out var state))
                return;
            
            if (_stateLocked)
            {
                _awaitedState = state;
                return;
            }

            Set(state);
        }

        public void Update()
        {
            CurrentState.Update();

            if (!_stateLocked)
                return;
            
            if (_lockTimeCounter > 0)
            {
                _lockTimeCounter -= Time.deltaTime;
                return;
            }

            _stateLocked = false;
            Set(_awaitedState);
        }

        public void FixedUpdate() => CurrentState.FixedUpdate();

        public void Dispose()
        {
            CurrentState?.Exit();
        }

        private void Set(FsmState state)
        {
            PreviousState = CurrentState;
            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
            
            Debug.Log(CurrentState);
        }
    }
}