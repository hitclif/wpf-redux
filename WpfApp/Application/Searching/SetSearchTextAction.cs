using Redux;

namespace WpfApp.Application.Searching
{
    public class SetSearchTextAction : IAction
    {
        public SetSearchTextAction(string searchText)
        {
            this.SearchText = searchText;
        }

        public string SearchText { get; }
    }
}
