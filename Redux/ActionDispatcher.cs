using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Redux
{
    public interface IActionDispatcher : IObservable<IAction>
    {
        void Dispatch(IAction action);
    }

    public class ActionDispatcher : IActionDispatcher
    {
        private readonly IScheduler _scheduler;
        private Subject<IAction> _subject = new Subject<IAction>();

        public ActionDispatcher()
        {
            _scheduler = Scheduler.CurrentThread;
        }

        public ActionDispatcher(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public void Dispatch(IAction action) => _subject.OnNext(action);

        public IDisposable Subscribe(IObserver<IAction> observer)
        {
            return _subject
                .ObserveOn(_scheduler)
                .Subscribe(observer);
        }
    }
}
