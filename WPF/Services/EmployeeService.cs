using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPF.Models;
using WPF.Repositories;

namespace WPF.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return _employeeRepository.GetAllEmployeesAsync();
        }

        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));
            
            // Business Validation
            if (string.IsNullOrWhiteSpace(employee.EmployeeCode)) return false;
            if (string.IsNullOrWhiteSpace(employee.FirstName)) return false;
            if (string.IsNullOrWhiteSpace(employee.LastName)) return false;

            int result = await _employeeRepository.AddEmployeeAsync(employee);
            return result > 0;
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));
            if (employee.Id <= 0) return false;
            
            // Business Validation
            if (string.IsNullOrWhiteSpace(employee.EmployeeCode)) return false;
            if (string.IsNullOrWhiteSpace(employee.FirstName)) return false;
            if (string.IsNullOrWhiteSpace(employee.LastName)) return false;

            int result = await _employeeRepository.UpdateEmployeeAsync(employee);
            return result > 0;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            if (id <= 0) return false;

            int result = await _employeeRepository.DeleteEmployeeAsync(id);
            return result > 0;
        }
    }
}
