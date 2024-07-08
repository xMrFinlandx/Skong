namespace Utilities.FSM
{
    public abstract class FsmState
    {
        protected FiniteStateMachine FiniteStateMachine { get; private set; }

        public FsmState(FiniteStateMachine finiteStateMachine)
        {
            FiniteStateMachine = finiteStateMachine;
        }

        public virtual void Enter() { }
        
        public virtual void Exit() { }

        public virtual void Update() { }

        public virtual void FixedUpdate() { }
    }
}