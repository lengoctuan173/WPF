using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPF.Domain.Models;
using WPF.Domain.Repositories;

namespace WPF.Infrastructure.Repositories
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employees;

        public MockEmployeeRepository()
        {
            // Seed initial mock data
            _employees = new List<Employee>
            {
                new Employee { Id = 1, EmployeeCode = "EMP001", FirstName = "An", LastName = "Nguyen", FullName = "Nguyen An", Gender = true },
                new Employee { Id = 2, EmployeeCode = "EMP002", FirstName = "Binh", LastName = "Tran", FullName = "Tran Binh", Gender = true },
                new Employee { Id = 3, EmployeeCode = "EMP003", FirstName = "Chi", LastName = "Le", FullName = "Le Chi", Gender = false }
            };
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return Task.FromResult<IEnumerable<Employee>>(_employees.ToList());
        }

        public Task<int> AddEmployeeAsync(Employee employee)
        {
            int newId = _employees.Any() ? _employees.Max(e => e.Id) + 1 : 1;
            employee.Id = newId;
            employee.FullName = $"{employee.LastName.Trim()} {employee.FirstName.Trim()}".Trim();
            _employees.Add(employee);
            return Task.FromResult(1);
        }

        public Task<int> UpdateEmployeeAsync(Employee employee)
        {
            var existing = _employees.FirstOrDefault(e => e.Id == employee.Id);
            if (existing != null)
            {
                existing.EmployeeCode = employee.EmployeeCode;
                existing.FirstName = employee.FirstName;
                existing.LastName = employee.LastName;
                existing.FullName = $"{employee.LastName.Trim()} {employee.FirstName.Trim()}".Trim();
                existing.Gender = employee.Gender;
                return Task.FromResult(1);
            }
            return Task.FromResult(0);
        }

        public Task<int> DeleteEmployeeAsync(int id)
        {
            var existing = _employees.FirstOrDefault(e => e.Id == id);
            if (existing != null)
            {
                _employees.Remove(existing);
                return Task.FromResult(1);
            }
            return Task.FromResult(0);
        }
    }
}
