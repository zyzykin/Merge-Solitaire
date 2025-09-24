namespace Whatwapp.Core.FSM
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}