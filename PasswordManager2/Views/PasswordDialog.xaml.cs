using PasswordManager2.Models.Password;
using PasswordManager2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PasswordManager2.Views
{
    /// <summary>
    /// Interaction logic for PasswordDialog.xaml
    /// </summary>
    public partial class PasswordDialog : Window
    {
        private readonly Random _random = new Random();
        private const string PasswordChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=[]{}|;:,.<>?";

        public CreatePasswordDto PasswordDto { get; private set; }

        public PasswordDialog(CreatePasswordDto existingPassword = null)
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;

            if (existingPassword != null)
            {
                TitleTextBox.Text = existingPassword.Title;
                UsernameTextBox.Text = existingPassword.Username;
                WebsiteTextBox.Text = existingPassword.Website;
                PasswordBox.Password = existingPassword.Password;
            }
        }

        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            var password = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                password.Append(PasswordChars[_random.Next(PasswordChars.Length)]);
            }
            PasswordBox.Password = password.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PasswordDto = new CreatePasswordDto
            {
                Title = TitleTextBox.Text.Trim(),
                Username = UsernameTextBox.Text.Trim(),
                Website = WebsiteTextBox.Text.Trim(),
                Password = PasswordBox.Password,
                IV = Convert.ToBase64String(GenerateIV())
            };

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private byte[] GenerateIV()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] iv = new byte[16];
                rng.GetBytes(iv);
                return iv;
            }
        }
    }
}
