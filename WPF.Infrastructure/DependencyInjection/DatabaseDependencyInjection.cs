using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WPF.Domain.Repositories;
using WPF.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DatabaseDependencyInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext Factory (EF Core) - Recommended for WPF stateful applications
            services.AddDbContextFactory<HRMDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("PAS")));

            // Register DbConnection Factory for Dapper
            string connString = configuration.GetConnectionString("PAS") ?? string.Empty;
            services.AddTransient<Func<IDbConnection>>(sp => () => new SqlConnection(connString));

            // Choose repository implementation (uncomment desired one)
            services.AddTransient<IEmployeeRepository, EfEmployeeRepository>(); // EF Core
            // services.AddTransient<IEmployeeRepository, DapperEmployeeRepository>(); // Dapper
            // services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>(); // Mock

            return services;
        }
    }
}
