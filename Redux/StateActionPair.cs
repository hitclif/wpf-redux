namespace Redux
{
    public class StateActionPair<T, IAction>
    {
        public StateActionPair(T state, IAction action)
        {
            this.State = state;
            this.Action = action;
        }

        public T State { get; }
        public IAction Action { get; }

        public static implicit operator T(StateActionPair<T, IAction> pair) => pair.State;
    }
}
