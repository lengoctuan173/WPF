using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WPF.Repositories;
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
            services.AddSingleton<IEmployeeRepository>(sp => new EmployeeRepository(AppConfig.ConnectionString));
            services.AddTransient<IEmployeeService, EmployeeService>();

            // Register ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<EmployeeViewModel>();

            // Register Windows
            services.AddTransient<LoginWindow>();
            services.AddTransient<EmployeeWindow>();

            ServiceProvider = services.BuildServiceProvider();

            // Launch first window using WindowService
            var windowService = ServiceProvider.GetRequiredService<IWindowService>();
            windowService.ShowLogin();
        }
    }
}
