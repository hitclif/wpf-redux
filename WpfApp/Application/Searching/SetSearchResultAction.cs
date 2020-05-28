using System.Collections.Generic;
using System.Linq;
using Redux;

namespace WpfApp.Application.Searching
{
    public class SetSearchResultsAction : IAction
    {
        public SetSearchResultsAction(IEnumerable<string> candidates)
        {
            this.Results = candidates.ToArray();
        }

        public IReadOnlyCollection<string> Results { get; }
    }
}
