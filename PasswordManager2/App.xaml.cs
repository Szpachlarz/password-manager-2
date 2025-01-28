using Microsoft.Extensions.Configuration;
using PasswordManager2.Helpers;
using PasswordManager2.Interfaces;
using PasswordManager2.Services;
using PasswordManager2.ViewModels;
using PasswordManager2.Views;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            containerRegistry.RegisterForNavigation<UserPanelView, UserPanelViewModel>();

            containerRegistry.RegisterSingleton<IAuthService, AuthService>();
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
            containerRegistry.RegisterSingleton<IPasswordService, PasswordService>();

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            containerRegistry.RegisterInstance<IConfiguration>(configuration);

            var encryptionKey = configuration["EncryptionSettings:Key"] ?? "YourEncryptionKey";
            var aesHelper = new AesEncryptionHelper(encryptionKey);
            containerRegistry.RegisterInstance(aesHelper);

            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = cookieContainer,
                AllowAutoRedirect = true
            };

            var httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            var baseUrl = configuration["ApiSettings:BaseUrl"];
            if (!string.IsNullOrEmpty(baseUrl))
            {
                httpClient.BaseAddress = new Uri(baseUrl);
            }

            containerRegistry.RegisterInstance(httpClient);

            containerRegistry.RegisterSingleton<IPasswordService>(() =>
        new PasswordService(
            httpClient,
            aesHelper
            )
        );
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

        protected override void OnExit(ExitEventArgs e)
        {
            var container = ContainerLocator.Current;
            if (container != null)
            {
                var httpClient = container.Resolve<HttpClient>();
                httpClient?.Dispose();
            }

            base.OnExit(e);
        }
    }
}
