using Redux;

namespace WpfApp.Application.Searching
{
    public static class SearchReducer
    {
        public static SearchState Reduce(SearchState state, IAction action)
        {
            switch (action)
            {
                case SetSearchTextAction act:
                    return state.UpdateSearch(act.SearchText);
                case SetSearchResultsAction res:
                    return state.UpdateSearchResults(res.Results);
            }

            return state;
        }
    }
}
