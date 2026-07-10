using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using WPF.Models;

namespace WPF.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            const string query = @"
                SELECT Id, EmployeeCode, FirstName, LastName, FullName, 
                       CASE 
                           WHEN Gender = 1 THEN 'Male' 
                           WHEN Gender = 0 THEN 'Female' 
                           ELSE 'Other' 
                       END AS Gender 
                FROM HRMEmployeesTest";

            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Employee>(query);
        }

        public async Task<int> AddEmployeeAsync(Employee employee)
        {
            const string query = @"
                INSERT INTO HRMEmployeesTest (EmployeeCode, FirstName, LastName, Gender) 
                VALUES (@EmployeeCode, @FirstName, @LastName, @Gender)";

            bool? dbGender = null;
            if (employee.Gender == "Male") dbGender = true;
            else if (employee.Gender == "Female") dbGender = false;

            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync(query, new
            {
                employee.EmployeeCode,
                employee.FirstName,
                employee.LastName,
                Gender = dbGender
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

            bool? dbGender = null;
            if (employee.Gender == "Male") dbGender = true;
            else if (employee.Gender == "Female") dbGender = false;

            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync(query, new
            {
                employee.Id,
                employee.EmployeeCode,
                employee.FirstName,
                employee.LastName,
                Gender = dbGender
            });
        }

        public async Task<int> DeleteEmployeeAsync(int id)
        {
            const string query = "DELETE FROM HRMEmployeesTest WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
