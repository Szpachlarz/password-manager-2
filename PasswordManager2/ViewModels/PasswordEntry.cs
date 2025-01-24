using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager2.ViewModels
{
    public class PasswordEntry : BindableBase
    {
        private int _id;
        private string _title;
        private string _username;
        private string _password;
        private string _website;
        private DateTime _lastModified;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string Website
        {
            get => _website;
            set => SetProperty(ref _website, value);
        }

        public DateTime LastModified
        {
            get => _lastModified;
            set => SetProperty(ref _lastModified, value);
        }

        public PasswordEntry Clone()
        {
            return new PasswordEntry
            {
                Id = this.Id,
                Title = this.Title,
                Username = this.Username,
                Password = this.Password,
                Website = this.Website,
                LastModified = this.LastModified
            };
        }
    }
}
