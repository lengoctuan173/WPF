using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WPF.Services;

namespace WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Setup Configuration (appsettings.json)
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration configuration = configBuilder.Build();

            var services = new ServiceCollection();

            // Register Configuration and Layer Extensions
            services.AddSingleton<IConfiguration>(configuration);
            services.AddDatabase(configuration);
            services.AddApplicationServices();
            services.AddViewModels();
            services.AddViews();

            ServiceProvider = services.BuildServiceProvider();

            // Launch first window using WindowService
            var windowService = ServiceProvider.GetRequiredService<IWindowService>();
            windowService.ShowLogin();
        }
    }
}
