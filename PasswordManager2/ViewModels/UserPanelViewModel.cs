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

namespace PasswordManager2.ViewModels
{
    internal class UserPanelViewModel : BindableBase
    {
        private readonly IAuthService _authService;
        private readonly IPasswordService _passwordService;
        private readonly IEventAggregator _eventAggregator;
        private ObservableCollection<PasswordEntry> _passwords;
        private PasswordEntry _selectedPassword;
        private string _currentUsername;
        private bool _isLoading;
        private string _errorMessage;

        public UserPanelViewModel(
            IAuthService authService,
            IPasswordService passwordService,
            IEventAggregator eventAggregator)
        {
            _authService = authService;
            _passwordService = passwordService;
            _eventAggregator = eventAggregator;

            Passwords = new ObservableCollection<PasswordEntry>();

            AddPasswordCommand = new DelegateCommand(ExecuteAddPassword);
            EditPasswordCommand = new DelegateCommand(ExecuteEditPassword, CanExecuteEditPassword);
            DeletePasswordCommand = new DelegateCommand(ExecuteDeletePassword, CanExecuteDeletePassword);
            RefreshCommand = new DelegateCommand(ExecuteRefresh);
            LogoutCommand = new DelegateCommand(ExecuteLogout);

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
                var newPassword = new PasswordEntry
                {
                    Title = "New Password",
                    Username = "",
                    Password = "",
                    Website = ""
                };

                var dialog = new PasswordDialog(newPassword);
                if (dialog.ShowDialog() == true)
                {
                    var passwordDto = newPassword.ToPasswordDto();
                    await _passwordService.AddPasswordAsync(passwordDto);
                    Passwords.Add(newPassword);
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

                var passwordToEdit = SelectedPassword.Clone();

                var dialog = new PasswordDialog(passwordToEdit);
                if (dialog.ShowDialog() == true)
                {
                    var passwordDto = passwordToEdit.ToPasswordDto();
                    await _passwordService.UpdatePasswordAsync(passwordDto);

                    var index = Passwords.IndexOf(SelectedPassword);
                    Passwords[index] = passwordToEdit;
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
                    "Are you sure you want to delete this password?",
                    "Confirm Delete",
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
                _eventAggregator.GetEvent<LogoutEvent>().Publish();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to logout: " + ex.Message;
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
    }
}
