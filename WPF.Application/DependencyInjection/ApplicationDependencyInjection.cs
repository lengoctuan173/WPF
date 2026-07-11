using Microsoft.Extensions.DependencyInjection;
using WPF.Application.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IEmployeeService, EmployeeService>();
            return services;
        }
    }
}
