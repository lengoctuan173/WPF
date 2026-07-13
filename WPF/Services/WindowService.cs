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

        public void ShowProduct()
        {
            var productWindow = _serviceProvider.GetRequiredService<ProductWindow>();
            var oldWindow = _currentWindow;
            _currentWindow = productWindow;
            productWindow.Show();
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
