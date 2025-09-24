using System;
using System.Collections.Generic;

namespace Whatwapp.Core.FSM
{
    public class StateMachine
    {
        private IState _currentState;
        private readonly Dictionary<Type, List<ITransition>> _transitions = new();
        private readonly List<ITransition> _anyTransitions = new();
        private List<ITransition> _currentTransitions = new();
        private static readonly List<ITransition> EmptyTransitions = new(0);

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<ITransition>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, condition));
        }

        public void AddAnyTransition(IState state, IPredicate condition)
        {
            _anyTransitions.Add(new Transition(state, condition));
        }

        public void SetState(IState state)
        {
            if (state == _currentState)
                return;

            _currentState?.OnExit();
            _currentState = state;

            _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
            _currentTransitions ??= EmptyTransitions;

            _currentState.OnEnter();
        }

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
                SetState(transition.To);

            _currentState?.Update();
        }

        public void FixedUpdate()
        {
            _currentState?.FixedUpdate();
        }

        private ITransition GetTransition()
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }

            foreach (var transition in _currentTransitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }

            return null;
        }
    }
}