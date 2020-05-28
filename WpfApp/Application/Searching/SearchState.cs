using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApp.Common;

namespace WpfApp.Application.Searching
{
    public class SearchState : Immutable<SearchState>
    {
        public static readonly SearchState Null = new SearchState();

        public string SearchText { get; private set; } = string.Empty;

        public IReadOnlyCollection<string> SearchResults { get; private set; } = new string[0];

        public SearchState UpdateSearch(string search)
        {
            return this.Update(
                s => s.SearchText = search,
                () => this.SearchText != search);
        }

        public SearchState UpdateSearchResults(IEnumerable<string> searchResults)
        {
            return this.Update(s => s.SearchResults = searchResults.ToArray());
        }

        public SearchState UpdateSearch(string search, IEnumerable<string> searchCandidates)
        {
            return this.Update(
                s => {
                    s.SearchText = search;
                    s.SearchResults = searchCandidates.ToArray();
                },
                () => this.SearchText != search);
        }
    }
}
