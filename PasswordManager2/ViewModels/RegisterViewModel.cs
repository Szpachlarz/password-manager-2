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
            bool isPasswordValid = IsPasswordValid(Password);

            if (!isPasswordValid && !string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Hasło musi mieć co najmniej osiem znaków, cyfry i znaki specjalne.";
            }
            else
            {
                ErrorMessage = string.Empty;
            }

            return !string.IsNullOrWhiteSpace(Username) &&
                   isPasswordValid &&
                   Password == ConfirmPassword &&
                   !IsLoading;
        }

        private async void ExecuteRegisterAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                if (!IsPasswordValid(Password))
                {
                    ErrorMessage = "Hasło musi mieć co najmniej osiem znaków, cyfry i znaki specjalne.";
                    return;
                }

                var result = await _authService.RegisterAsync(Username, Password);

                if (result.Success)
                {
                    _regionManager.RequestNavigate("MainRegion", "LoginView");
                    ClearUserData();
                }
                else
                {
                    ErrorMessage = result.Message;
                }
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

        private bool IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            bool hasNumber = password.Any(char.IsDigit);
            bool hasLetter = password.Any(char.IsLetter);
            bool hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));

            return hasNumber && hasLetter && hasSpecial;
        }

        private void NavigateToLogin()
        {
            _regionManager.RequestNavigate("MainRegion", "LoginView");
            ClearUserData();
        }

        private void ClearUserData()
        {
            Username = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            ErrorMessage = string.Empty;
        }
    }
}
