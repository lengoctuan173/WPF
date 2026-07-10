using System;
using System.Windows;
using System.Windows.Input;
using WPF.ViewModels;

namespace WPF.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                // Synchronize password changes from PasswordBox to ViewModel
                vm.LoginForm.Password = txtPassword.Password;
            }
        }
    }
}
