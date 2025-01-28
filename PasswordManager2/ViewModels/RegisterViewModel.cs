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
    public class RegisterViewModel : BindableBase
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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
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

        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        public RegisterViewModel(IAuthService authService, IRegionManager regionManager)
        {
            _authService = authService;
            _regionManager = regionManager;
            RegisterCommand = new DelegateCommand(ExecuteRegisterAsync, CanExecuteRegister)
                .ObservesProperty(() => Username)
                .ObservesProperty(() => Password)
                .ObservesProperty(() => ConfirmPassword);
            NavigateToLoginCommand = new DelegateCommand(NavigateToLogin);
        }

        private bool CanExecuteRegister()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   Password == ConfirmPassword &&
                   !IsLoading;
        }

        private async void ExecuteRegisterAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                await _authService.RegisterAsync(Username, Password);
                _regionManager.RequestNavigate("MainRegion", "LoginView");
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

        private void NavigateToLogin()
        {
            _regionManager.RequestNavigate("MainRegion", "LoginView");
        }
    }
}
