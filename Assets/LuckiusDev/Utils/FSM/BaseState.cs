
namespace LuckiusDev.FSM
{
    public abstract class BaseState
    {
        public string Name { get; private set; }

        protected StateMachine m_StateMachine;

        protected BaseState( string name, StateMachine stateMachine ) {
            Name = name;
            m_StateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void UpdateLogic() { }
        public virtual void UpdatePhysics() { }
        public virtual void Exit() { }
    }
}