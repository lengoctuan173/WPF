using Microsoft.Extensions.DependencyInjection;
using WPF.ViewModels;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ViewModelDependencyInjection
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddTransient<LoginViewModel>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<ProductViewModelToolkit>();
            return services;
        }
    }
}
