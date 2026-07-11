using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using WPF.Domain.Models;
using WPF.Domain.Repositories;

namespace WPF.Infrastructure.Repositories
{
    public class DapperEmployeeRepository : IEmployeeRepository
    {
        private readonly Func<IDbConnection> _connectionFactory;

        public DapperEmployeeRepository(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            const string query = "SELECT Id, EmployeeCode, FirstName, LastName, FullName, Gender FROM HRMEmployeesTest";

            using var connection = _connectionFactory();
            return await connection.QueryAsync<Employee>(query);
        }

        public async Task<int> AddEmployeeAsync(Employee employee)
        {
            const string query = @"
                INSERT INTO HRMEmployeesTest (EmployeeCode, FirstName, LastName, Gender) 
                VALUES (@EmployeeCode, @FirstName, @LastName, @Gender)";

            using var connection = _connectionFactory();
            return await connection.ExecuteAsync(query, new
            {
                employee.EmployeeCode,
                employee.FirstName,
                employee.LastName,
                employee.Gender
            });
        }

        public async Task<int> UpdateEmployeeAsync(Employee employee)
        {
            const string query = @"
                UPDATE HRMEmployeesTest 
                SET EmployeeCode = @EmployeeCode, 
                    FirstName = @FirstName, 
                    LastName = @LastName, 
                    Gender = @Gender 
                WHERE Id = @Id";

            using var connection = _connectionFactory();
            return await connection.ExecuteAsync(query, new
            {
                employee.Id,
                employee.EmployeeCode,
                employee.FirstName,
                employee.LastName,
                employee.Gender
            });
        }

        public async Task<int> DeleteEmployeeAsync(int id)
        {
            const string query = "DELETE FROM HRMEmployeesTest WHERE Id = @Id";

            using var connection = _connectionFactory();
            return await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
