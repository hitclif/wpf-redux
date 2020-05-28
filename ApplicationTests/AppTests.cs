using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redux;
using WpfApp.Application;
using WpfApp.Application.Searching;

namespace ApplicationTests
{
    [TestClass]
    public class AppTests
    {
        private ActionDispatcher _dispatcher;
        private Store<RootState> _store;

        public AppTests()
        {
            _dispatcher = new ActionDispatcher();
            _store = new Store<RootState>();

            _store.Initialize(
                new RootState(),
                RootReducer.Reduce,
                _dispatcher,
                new Effect<RootState>[0]);
        }

        [TestMethod]
        public void TestStoreWithActionStream()
        {
            // act
            _dispatcher.Dispatch(new SetSearchTextAction("s"));
            _dispatcher.Dispatch(new SetSearchTextAction("se"));
            _dispatcher.Dispatch(new SetSearchTextAction("sea"));
            _dispatcher.Dispatch(new SetSearchTextAction("sear"));
            _dispatcher.Dispatch(new SetSearchTextAction("searc"));
            _dispatcher.Dispatch(new SetSearchTextAction("search"));

            _dispatcher.Dispatch(new SetSearchResultsAction(new[] { "the", "eventual", "results" }));

            // assert
            RootState resultingState = null;
            _store.Subscribe(rs => {
                resultingState = rs;
            });

            Assert.IsNotNull(resultingState);
            Assert.IsNotNull(resultingState.Search);
            Assert.AreEqual("search", resultingState.Search.SearchText);

            foreach (var expected in new[] { "the", "eventual", "results" })
            {
                Assert.IsTrue(resultingState.Search.SearchResults.Any(sr => sr == expected));
            }
        }

        [TestMethod]
        public void TestRootReducer()
        {
            // prepare
            var state = new RootState();

            // act
            var resultState = RootReducer.Reduce(state, new SetSearchTextAction("xxx"));

            // assert
            Assert.AreEqual(resultState.Search.SearchText, "xxx");
        }

        [TestMethod]
        public void TestSearchReducer()
        {
            // prepare
            var state = new SearchState();

            // act
            var resultState = SearchReducer.Reduce(state, new SetSearchTextAction("xxx"));

            // assert
            Assert.AreEqual(resultState.SearchText, "xxx");
        }
    }
}
