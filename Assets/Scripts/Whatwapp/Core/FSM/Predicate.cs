using System;

namespace Whatwapp.Core.FSM
{
    public class Predicate : IPredicate
    {
        private readonly Func<bool> _condition;

        public Predicate(Func<bool> condition)
        {
            _condition = condition;
        }

        public bool Evaluate()
        {
            return _condition();
        }
    }
}