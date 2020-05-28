using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using GalaSoft.MvvmLight;
using Redux;
using WpfApp.Application;
using WpfApp.Application.Searching;

namespace WpfApp.Views
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IObservable<RootState> _stateStream;
        private readonly IActionDispatcher _actionDispatcher;

        private IDisposable _subscription;
        private SearchState _state = SearchState.Null;


        public MainWindowViewModel(
            IObservable<RootState> state,
            IActionDispatcher actionDispatcher)
        {
            _stateStream = state;
            _actionDispatcher = actionDispatcher;
        }

        public string SearchText
        {
            get => _state.SearchText;
            set
            {
                _actionDispatcher.Dispatch(new SetSearchTextAction(value));
            }
        }

        public IReadOnlyCollection<string> SearchResults
        {
            get => _state.SearchResults;
        }

        public void Connect()
        {
            _subscription = _stateStream
                .Select(root => root.Search)
                .Subscribe(newState =>
                {
                    _state = newState;

                    this.RaisePropertyChanged(nameof(SearchText));
                    this.RaisePropertyChanged(nameof(SearchResults));
                });

            // later on don't forget to dispose the subscription
        }
    }
}
