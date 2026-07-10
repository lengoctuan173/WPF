using System;
using System.Windows.Input;
using System.Windows.Media;
using WPF.Services;

namespace WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private Brush _errorColor = Brushes.Red;
        private bool _isShowPassword;

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

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public Brush ErrorColor
        {
            get => _errorColor;
            set => SetProperty(ref _errorColor, value);
        }

        public bool IsShowPassword
        {
            get => _isShowPassword;
            set
            {
                if (SetProperty(ref _isShowPassword, value))
                {
                    OnPropertyChanged(nameof(PasswordVisibility));
                    OnPropertyChanged(nameof(PasswordPlainVisibility));
                }
            }
        }

        public System.Windows.Visibility PasswordVisibility => IsShowPassword ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
        public System.Windows.Visibility PasswordPlainVisibility => IsShowPassword ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

        public ICommand LoginCommand { get; }
        public ICommand ExitCommand { get; }

        private readonly IWindowService _windowService;

        public LoginViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            LoginCommand = new RelayCommand(ExecuteLogin);
            ExitCommand = new RelayCommand(ExecuteExit);
        }

        private void ExecuteLogin()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrEmpty(Username))
            {
                ErrorColor = new SolidColorBrush(Color.FromRgb(220, 53, 69));
                ErrorMessage = "Please enter your username.";
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                ErrorColor = new SolidColorBrush(Color.FromRgb(220, 53, 69));
                ErrorMessage = "Please enter your password.";
                return;
            }

            if (Username == "admin" && Password == "admin123")
            {
                ErrorColor = new SolidColorBrush(Color.FromRgb(40, 167, 69));
                ErrorMessage = "Login successful!";

                System.Windows.MessageBox.Show("Welcome back, Admin!", "Login Successful", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                _windowService.ShowHome();

                // Clear credentials for security when hidden
                Password = string.Empty;
                ErrorMessage = string.Empty;
            }
            else
            {
                ErrorColor = new SolidColorBrush(Color.FromRgb(220, 53, 69));
                ErrorMessage = "Invalid username or password.";
                Password = string.Empty;
            }
        }

        private void ExecuteExit()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
