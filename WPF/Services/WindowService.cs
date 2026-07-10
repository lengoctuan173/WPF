using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WPF.Views;

namespace WPF.Services
{
    public class WindowService : IWindowService
    {
        private readonly IServiceProvider _serviceProvider;
        private Window? _currentWindow;

        public WindowService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ShowEmployee()
        {
            var employeeWindow = _serviceProvider.GetRequiredService<EmployeeWindow>();
            var oldWindow = _currentWindow;
            _currentWindow = employeeWindow;
            employeeWindow.Show();
            oldWindow?.Close();
        }

        public void ShowLogin()
        {
            var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            var oldWindow = _currentWindow;
            _currentWindow = loginWindow;
            loginWindow.Show();
            oldWindow?.Close();
        }
    }
}
