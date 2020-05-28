using System;
using System.Reactive.Linq;

namespace Redux
{
    public interface IEffect<T>
    {
        void Connect(IObservable<StateActionPair<T, IAction>> stateActionStream);
        void Disconnect();
    }

    public abstract class Effect<T> : IEffect<T>
    {
        public void Connect(IObservable<StateActionPair<T, IAction>> stateActionStream)
        {
            this.ActionState = stateActionStream;
            this.Subscribe();
        }

        public void Disconnect()
        {
            this.Unsubsribe();
            this.ActionState = null;
        }

        protected IObservable<StateActionPair<T, IAction>> ActionState { get; private set; }

        protected IObservable<T> State => this.ActionState.Select(pair => pair.State);

        protected IObservable<IAction> Action => this.ActionState.Select(pair => pair.Action);

        protected IObservable<TAction> ActionOfType<TAction>()
            where TAction : IAction
        {
            return this.Action
                .Where(a => a is TAction)
                .Cast<TAction>();
        }

        protected abstract void Subscribe();
        protected abstract void Unsubsribe();
    }
}
