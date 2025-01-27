using PasswordManager2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace PasswordManager2.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IAuthService _authService;
        private readonly IRegionManager _regionManager;
        private string _username;
        private string _errorMessage;
        private bool _isLoading;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public LoginViewModel(IAuthService authService, IRegionManager regionManager)
        {
            _authService = authService;
            _regionManager = regionManager;
            LoginCommand = new DelegateCommand(ExecuteLoginAsync, CanExecuteLogin)
                .ObservesProperty(() => Username)
                .ObservesProperty(() => Password);
            NavigateToRegisterCommand = new DelegateCommand(NavigateToRegister);
        }

        private bool CanExecuteLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !IsLoading;
        }

        private async void ExecuteLoginAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                await _authService.LoginAsync(Username, Password);
                _regionManager.RequestNavigate("MainRegion", "Home"); ;
            }
            catch (AuthenticationException ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void NavigateToRegister()
        {
            _regionManager.RequestNavigate("MainRegion", "Register");
        }
    }
}
