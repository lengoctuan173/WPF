using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WPF.Domain.Models;
using WPF.Domain.Repositories;

namespace WPF.Infrastructure.Repositories
{
    public class EfEmployeeRepository : IEmployeeRepository
    {
        private readonly IDbContextFactory<HRMDbContext> _contextFactory;

        public EfEmployeeRepository(IDbContextFactory<HRMDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Employees.AsNoTracking().ToListAsync();
        }

        public async Task<int> AddEmployeeAsync(Employee employee)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Employees.Add(employee);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateEmployeeAsync(Employee employee)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var existing = await context.Employees.FindAsync(employee.Id);
            if (existing != null)
            {
                context.Entry(existing).CurrentValues.SetValues(employee);
                return await context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> DeleteEmployeeAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var employee = await context.Employees.FindAsync(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                return await context.SaveChangesAsync();
            }
            return 0;
        }
    }
}
