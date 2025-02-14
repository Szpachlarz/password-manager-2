using PasswordManager2.Interfaces;
using PasswordManager2.Mappers;
using PasswordManager2.Views;
using PasswordManager2.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PasswordManager2.Models.Password;

namespace PasswordManager2.ViewModels
{
    public class UserPanelViewModel : BindableBase, INavigationAware
    {
        private readonly IAuthService _authService;
        private readonly IPasswordService _passwordService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private ObservableCollection<PasswordEntry> _passwords;
        private PasswordEntry _selectedPassword;
        private string _currentUsername;
        private bool _isLoading;
        private string _errorMessage;

        public UserPanelViewModel(
            IAuthService authService,
            IPasswordService passwordService,
            IEventAggregator eventAggregator,
            IRegionManager regionManager)
        {
            _authService = authService;
            _passwordService = passwordService;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            Passwords = new ObservableCollection<PasswordEntry>();

            AddPasswordCommand = new DelegateCommand(ExecuteAddPassword);
            EditPasswordCommand = new DelegateCommand(ExecuteEditPassword, CanExecuteEditPassword);
            DeletePasswordCommand = new DelegateCommand(ExecuteDeletePassword, CanExecuteDeletePassword);
            RefreshCommand = new DelegateCommand(ExecuteRefresh);
            LogoutCommand = new DelegateCommand(ExecuteLogout);
            CopyPasswordCommand = new DelegateCommand<PasswordEntry>(ExecuteCopyPassword);

            LoadUserDataAsync();
        }

        public ObservableCollection<PasswordEntry> Passwords
        {
            get => _passwords;
            set => SetProperty(ref _passwords, value);
        }

        public PasswordEntry SelectedPassword
        {
            get => _selectedPassword;
            set
            {
                SetProperty(ref _selectedPassword, value);
                EditPasswordCommand.RaiseCanExecuteChanged();
                DeletePasswordCommand.RaiseCanExecuteChanged();
            }
        }

        public string CurrentUsername
        {
            get => _currentUsername;
            set => SetProperty(ref _currentUsername, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public DelegateCommand AddPasswordCommand { get; }
        public DelegateCommand EditPasswordCommand { get; }
        public DelegateCommand DeletePasswordCommand { get; }
        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand LogoutCommand { get; }
        public DelegateCommand<PasswordEntry> CopyPasswordCommand { get; }

        private async void LoadUserDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                CurrentUsername = await _authService.GetCurrentUserAsync();

                var passwords = await _passwordService.GetPasswordsAsync();
                Passwords.Clear();
                foreach (var password in passwords)
                {
                    Passwords.Add(password.ToPasswordEntry());
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load data: " + ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void ExecuteAddPassword()
        {
            try
            {
                var dialog = new PasswordDialog();
                if (dialog.ShowDialog() == true)
                {
                    await _passwordService.AddPasswordAsync(dialog.PasswordDto);

                    LoadUserDataAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to add password: " + ex.Message;
            }
        }

        private async void ExecuteEditPassword()
        {
            try
            {
                if (SelectedPassword == null) return;

                var passwordDto = new CreatePasswordDto
                {
                    Title = SelectedPassword.Title,
                    Username = SelectedPassword.Username,
                    Website = SelectedPassword.Website,
                    Password = SelectedPassword.Password 
                };

                var dialog = new PasswordDialog(passwordDto);
                if (dialog.ShowDialog() == true)
                {
                    await _passwordService.UpdatePasswordAsync(SelectedPassword.Id, dialog.PasswordDto);

                    LoadUserDataAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to edit password: " + ex.Message;
            }
        }

        private async void ExecuteDeletePassword()
        {
            try
            {
                if (SelectedPassword == null) return;

                var result = MessageBox.Show(
                    "Czy na pewno chcesz usunąć to hasło?",
                    "Usuń",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    await _passwordService.DeletePasswordAsync(SelectedPassword.Id);
                    Passwords.Remove(SelectedPassword);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to delete password: " + ex.Message;
            }
        }

        private void ExecuteRefresh()
        {
            LoadUserDataAsync();
        }

        private async void ExecuteLogout()
        {
            try
            {
                await _authService.LogoutAsync();
                ClearUserData();
                _regionManager.RequestNavigate("MainRegion", "HomeView");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to logout: " + ex.Message;
            }
        }

        private async void ExecuteCopyPassword(PasswordEntry password)
        {
            try
            {
                if (password == null) return;

                Clipboard.SetText(password.Password);

                var originalError = ErrorMessage;
                ErrorMessage = "Hasło skopiowane do schowka";
                await Task.Delay(2000);
                ErrorMessage = originalError;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to copy password: " + ex.Message;
            }
        }

        private bool CanExecuteEditPassword()
        {
            return SelectedPassword != null;
        }

        private bool CanExecuteDeletePassword()
        {
            return SelectedPassword != null;
        }

        private void ClearUserData()
        {
            CurrentUsername = null;
            Passwords.Clear();
            SelectedPassword = null;
            ErrorMessage = string.Empty;
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadUserDataAsync();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
