using PasswordManager2.Interfaces;
using PasswordManager2.Services;
using PasswordManager2.ViewModels;
using PasswordManager2.Views;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System.Windows;
using System.Windows.Controls;

namespace PasswordManager2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var window = Container.Resolve<MainWindow>();
            window.DataContext = Container.Resolve<MainWindowViewModel>();
            return window;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>();
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<RegisterView, RegisterViewModel>();
            //containerRegistry.RegisterForNavigation<HomeView>("Home");
            //containerRegistry.RegisterForNavigation<LoginView>("Login");
            //containerRegistry.RegisterForNavigation<RegisterView>("Register");

            containerRegistry.RegisterSingleton<IAuthService, AuthService>();
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var regionManager = Container.Resolve<IRegionManager>();
                regionManager.RequestNavigate("MainRegion", "HomeView");
            });
        }
    }
}
