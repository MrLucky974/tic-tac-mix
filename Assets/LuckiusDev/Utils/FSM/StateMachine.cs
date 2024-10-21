using UnityEngine;

namespace LuckiusDev.FSM
{
    public class StateMachine : MonoBehaviour
    {
        private BaseState _currentState;

        protected virtual void Start() {
            _currentState = GetInitialState();
            _currentState?.Enter();
        }

        protected virtual void Update()
        {
            _currentState?.UpdateLogic();
        }

        protected virtual void FixedUpdate()
        {
            _currentState?.UpdatePhysics();
        }

        protected virtual BaseState GetInitialState() {
            return null;
        }

        public void ChangeState( BaseState newState ) {
            _currentState.Exit();

            _currentState = newState;
            newState.Enter();
        }

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(10f, 10f, 200f, 100f));
            var content = _currentState != null ? _currentState.Name : "(no current state)";
            GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
            GUILayout.EndArea();
        }
    }
}