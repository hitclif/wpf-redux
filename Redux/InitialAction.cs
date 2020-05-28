namespace Redux
{
    public class InitialAction : IAction
    {
        internal static readonly InitialAction Instance = new InitialAction();

        private InitialAction()
        {
        }
    }
}
