using System;
using System.Windows.Media;

namespace WPF.ViewModels.Forms
{
    public class LoginFormViewModel : ViewModelBase
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

        public void Clear()
        {
            Username = string.Empty;
            Password = string.Empty;
            ErrorMessage = string.Empty;
            IsShowPassword = false;
            ErrorColor = Brushes.Red;
        }
    }
}
