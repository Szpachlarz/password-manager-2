using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager2.ViewModels
{
    public class PasswordDialogViewModel : INotifyPropertyChanged
    {
        private PasswordEntry _passwordEntry;

        public PasswordDialogViewModel(PasswordEntry passwordEntry)
        {
            _passwordEntry = passwordEntry.Clone();
        }

        public string Title
        {
            get => _passwordEntry.Title;
            set
            {
                _passwordEntry.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Username
        {
            get => _passwordEntry.Username;
            set
            {
                _passwordEntry.Username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _passwordEntry.Password;
            set
            {
                _passwordEntry.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Website
        {
            get => _passwordEntry.Website;
            set
            {
                _passwordEntry.Website = value;
                OnPropertyChanged(nameof(Website));
            }
        }

        public PasswordEntry GetPasswordEntry() => _passwordEntry;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
