using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WPF.Services;
using WPF.ViewModels;
using WPF.Views;

namespace WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            // Register Services
            services.AddSingleton<IWindowService, WindowService>();

            // Register ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<HomeViewModel>();

            // Register Windows
            services.AddTransient<LoginWindow>();
            services.AddTransient<HomeWindow>();

            ServiceProvider = services.BuildServiceProvider();

            // Launch first window using WindowService
            var windowService = ServiceProvider.GetRequiredService<IWindowService>();
            windowService.ShowLogin();
        }
    }
}
