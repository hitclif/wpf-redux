using WpfApp.Application.Searching;
using WpfApp.Common;

namespace WpfApp.Application
{
    public class RootState : Immutable<RootState>
    {
        public SearchState Search { get; private set; } = SearchState.Null;

        internal RootState UpdateSearch(SearchState search)
        {
            return this.Search == search
                ? this
                : this.Update(s => s.Search = search);
        }
    }
}
