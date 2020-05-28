using System;
using System.Windows;
using Redux;
using SimpleInjector;
using WpfApp.Application;
using WpfApp.Application.Searching;
using WpfApp.Views;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private Container _container;

        public App()
        {
            _container = new Container();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.Bootstrap();

            var mainViewModel = _container.GetInstance<MainWindowViewModel>();
            mainViewModel.Connect();

            MainWindow window = new MainWindow();
            window.DataContext = mainViewModel;
            window.Show();
        }

        private void Bootstrap()
        {
            var actionDispatcher = new ActionDispatcher();

            var store = new Store<RootState>();
            store.Initialize(
                new RootState(),
                RootReducer.Reduce,
                actionDispatcher,
                new[] { new SearchEffect(actionDispatcher) });

            _container.RegisterInstance<IActionDispatcher>(actionDispatcher);
            _container.RegisterInstance<IObservable<RootState>>(store);

            _container.Register<MainWindowViewModel>();
        }
    }
}
