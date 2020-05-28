using Redux;

namespace WpfApp.Application
{
    public static class RootReducer
    {
        public static RootState Reduce(RootState state, IAction action)
        {
            var search = Searching.SearchReducer.Reduce(state.Search, action);

            var newState = state
                .UpdateSearch(search);

            return newState;
        }
    }
}
