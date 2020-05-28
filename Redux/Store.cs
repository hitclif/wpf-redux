using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Redux
{
    public sealed class Store<T> : IObservable<T>, IDisposable
    {
        private IDisposable _subscription;
        private Subject<StateActionPair<T, IAction>> _effectStream = new Subject<StateActionPair<T, IAction>>();
        private BehaviorSubject<T> _behaviorSubject = new BehaviorSubject<T>(default);
        private Effect<T>[] _effects;

        public void Initialize(
            T initialState,
            Func<T, IAction, T> rootReducer,
            IActionDispatcher dispatcher,
            IEnumerable<Effect<T>> effects)
        {
            _effects = effects.ToArray();

            var seed = new StateActionPair<T, IAction>(initialState, null);

            _subscription = dispatcher
                .Do(action => Debug.WriteLine(action))
                .Scan(seed, (StateActionPair<T, IAction> accumulator, IAction action) => {
                    var newState = rootReducer(accumulator.State, action);
                    return new StateActionPair<T, IAction>(newState, action);
                })
                .Subscribe(pair =>
                {
                    _behaviorSubject.OnNext(pair.State);
                    _effectStream.OnNext(pair);
                });

            foreach (var ef in _effects)
            {
                ef.Connect(_effectStream);
            }

            dispatcher.Dispatch(InitialAction.Instance);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _behaviorSubject.Subscribe(observer);
        }

        public void Dispose()
        {
            _effectStream?.Dispose();
            _effectStream = null;

            _subscription?.Dispose();
            _subscription = null;
        }
    }
}
