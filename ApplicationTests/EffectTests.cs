using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redux;
using WpfApp.Application;
using WpfApp.Application.Searching;

namespace ApplicationTests
{
    [TestClass]
    public class EffectTests
    {
        private readonly Subject<StateActionPair<RootState, IAction>> _stateActionStream;
        private readonly TestScheduler _scheduler;
        private readonly JournalingActionDispatcher _actionDispatcher;
        private readonly SearchEffect _testee;

        public EffectTests()
        {
            _stateActionStream = new Subject<StateActionPair<RootState, IAction>>();
            _scheduler = new TestScheduler();
            _actionDispatcher = new JournalingActionDispatcher();

            _testee = new SearchEffect(_actionDispatcher, TimeSpan.FromMilliseconds(200), _scheduler);
            _testee.Connect(_stateActionStream);
        }

        [TestMethod]
        public void TestSearchEffect_DispatchAction()
        {
            // act
            _stateActionStream.OnNext(new StateActionPair<RootState, IAction>(new RootState(), new SetSearchTextAction("si")));
            _scheduler.AdvanceBy(TimeSpan.FromMilliseconds(210).Ticks); // a bit more then the sampling limit

            // assert
            var journal = _actionDispatcher.GetActionJournal().ToList();
            Assert.AreEqual(1, journal.Count);

            var action = journal[0] as SetSearchResultsAction;
            Assert.IsNotNull(action);
            Assert.IsTrue(action.Results.Contains("simple"));
            Assert.IsTrue(action.Results.Contains("using"));
        }

        [TestMethod]
        public void TestSearchEffect_NoDispatch()
        {
            // act
            _stateActionStream.OnNext(new StateActionPair<RootState, IAction>(new RootState(), new SetSearchTextAction("si")));
            _scheduler.AdvanceBy(TimeSpan.FromMilliseconds(100).Ticks); // less then the sampling limit

            // assert
            var journal = _actionDispatcher.GetActionJournal().ToList();
            Assert.AreEqual(0, journal.Count);
        }

        private class JournalingActionDispatcher : IActionDispatcher
        {
            private ReplaySubject<IAction> _subject;

            public JournalingActionDispatcher(int bufferSize = 1000)
            {
                _subject = new ReplaySubject<IAction>(bufferSize);
            }

            public void Dispatch(IAction action)
            {
                _subject.OnNext(action);
            }

            public IEnumerable<IAction> GetActionJournal()
            {
                var actionJournal = new List<IAction>();

                var s = _subject
                    .Subscribe(a => actionJournal.Add(a));
                s.Dispose();

                return actionJournal;
            }

            public IDisposable Subscribe(IObserver<IAction> observer)
            {
                return _subject.Subscribe(observer);
            }
        }
    }
}
