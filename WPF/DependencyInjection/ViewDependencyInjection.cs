using Microsoft.Extensions.DependencyInjection;
using WPF.Services;
using WPF.Views;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ViewDependencyInjection
    {
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            // Window Management Service
            services.AddSingleton<IWindowService, WindowService>();

            // Windows
            services.AddTransient<LoginWindow>();
            services.AddTransient<EmployeeWindow>();

            return services;
        }
    }
}
