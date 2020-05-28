using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Redux;

namespace WpfApp.Application.Searching
{
    public class SearchEffect : Effect<RootState>
    {
        private readonly string[] _words = { "some", "simple", "words", "to", "test", "the", "search", "functionality", "using", "an", "effect" };

        private readonly IActionDispatcher _actionDispatcher;
        private readonly TimeSpan _sampling;

        // the scheduler is necessary due to unit testing
        private readonly IScheduler _scheduler;
        private IDisposable _subscription;

        public SearchEffect(IActionDispatcher actionDispatcher) : this(actionDispatcher, TimeSpan.FromMilliseconds(400), Scheduler.Default)
        {
            // defualt scheduler => NewThread
        }

        public SearchEffect(IActionDispatcher actionDispatcher, TimeSpan sampling, IScheduler scheduler)
        {
            _actionDispatcher = actionDispatcher;
            _sampling = sampling;
            _scheduler = scheduler;
        }

        protected override void Subscribe()
        {
            _subscription = this
                .ActionState
                .Where(pair => pair.Action is SetSearchTextAction)
                .Select(pair => pair.Action)
                .OfType<SetSearchTextAction>()
                .Sample(_sampling, _scheduler)
                .Select(action => 
                {
                    var candidates = this.FindCandidates(action.SearchText);
                    return new SetSearchResultsAction(candidates);
                })
                .Subscribe(action => {
                    _actionDispatcher.Dispatch(action);
                });
        }

        protected override void Unsubsribe()
        {
            _subscription?.Dispose();
        }

        private IEnumerable<string> FindCandidates(string searchText)
        {
            var results = _words
                .Where(w => Regex.IsMatch(w, searchText))
                .ToArray();

            return results;
        }
    }
}
