using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;
using System.Windows.Media;
using WPF.Services;
using WPF.ViewModels.Forms;

namespace WPF.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        public LoginFormViewModel LoginForm { get; } = new();

        private readonly IWindowService _windowService;

        public LoginViewModel(IWindowService windowService)
        {
            _windowService = windowService;
        }

        [RelayCommand]
        private void Login()
        {
            LoginForm.ErrorMessage = string.Empty;

            if (string.IsNullOrEmpty(LoginForm.Username))
            {
                LoginForm.ErrorColor = new SolidColorBrush(Color.FromRgb(220, 53, 69));
                LoginForm.ErrorMessage = "Please enter your username.";
                return;
            }

            if (string.IsNullOrEmpty(LoginForm.Password))
            {
                LoginForm.ErrorColor = new SolidColorBrush(Color.FromRgb(220, 53, 69));
                LoginForm.ErrorMessage = "Please enter your password.";
                return;
            }

            if (LoginForm.Username == "admin" && LoginForm.Password == "123456")
            {
                LoginForm.ErrorColor = new SolidColorBrush(Color.FromRgb(40, 167, 69));
                LoginForm.ErrorMessage = "Login successful!";

                System.Windows.MessageBox.Show("Welcome back, Admin!", "Login Successful", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                _windowService.ShowProduct();

                // Clear credentials for security when hidden
                LoginForm.Clear();
            }
            else
            {
                LoginForm.ErrorColor = new SolidColorBrush(Color.FromRgb(220, 53, 69));
                LoginForm.ErrorMessage = "Invalid username or password.";
                LoginForm.Password = string.Empty;
            }
        }

        [RelayCommand]
        private void Exit()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
