using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Dapper;
using Microsoft.Data.SqlClient;
using WPF.Models;
using WPF.Services;
using WPF.ViewModels.Forms;

namespace WPF.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private ObservableCollection<Employee> _employees = new();
        private Employee? _selectedEmployee;
        
        private string _statusMessage = "Connection status: Ready OK";
        private Brush _statusColor = Brushes.Gray;

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set => SetProperty(ref _employees, value);
        }

        public Employee? SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                if (SetProperty(ref _selectedEmployee, value))
                {
                    OnSelectedEmployeeChanged();
                }
            }
        }

        public EmployeeFormViewModel EmployeeForm { get; } = new();

        public ObservableCollection<string> Genders { get; } = new() { "Male", "Female" };

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public Brush StatusColor
        {
            get => _statusColor;
            set => SetProperty(ref _statusColor, value);
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand LogoutCommand { get; }

        private readonly IWindowService _windowService;

        public HomeViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            AddCommand = new RelayCommand(ExecuteAdd);
            UpdateCommand = new RelayCommand(ExecuteUpdate);
            DeleteCommand = new RelayCommand(ExecuteDelete);
            LogoutCommand = new RelayCommand(ExecuteLogout);

            LoadEmployeeData();
        }

        public void LoadEmployeeData()
        {
            string query = "SELECT Id, EmployeeCode, FirstName, LastName, FullName, " +
                           "CASE WHEN Gender = 1 THEN 'Male' WHEN Gender = 0 THEN 'Female' ELSE 'Other' END AS Gender " +
                           "FROM HRMEmployees";

            try
            {
                using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString))
                {
                    var list = connection.Query<Employee>(query);
                    Employees = new ObservableCollection<Employee>(list);
                }
                StatusMessage = "Data loaded successfully from HRM database.";
                StatusColor = Brushes.Gray;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to database:\n{ex.Message}", "Database Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusMessage = "Failed to load employee list. Check database connection settings.";
                StatusColor = Brushes.Red;
            }
        }

        private void OnSelectedEmployeeChanged()
        {
            if (SelectedEmployee != null)
            {
                EmployeeForm.EmployeeCode = SelectedEmployee.EmployeeCode;
                EmployeeForm.FirstName = SelectedEmployee.FirstName;
                EmployeeForm.LastName = SelectedEmployee.LastName;
                EmployeeForm.Gender = SelectedEmployee.Gender;
            }
            else
            {
                ClearInputs();
            }
        }

        private void ClearInputs()
        {
            EmployeeForm.Clear();
        }

        private void ExecuteAdd()
        {
            string code = EmployeeForm.EmployeeCode.Trim();
            string first = EmployeeForm.FirstName.Trim();
            string last = EmployeeForm.LastName.Trim();
            string full = EmployeeForm.FullName.Trim();
            string gender = EmployeeForm.Gender;

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(first) || string.IsNullOrEmpty(last))
            {
                MessageBox.Show("Please fill Employee Code, First Name, and Last Name.", "Validation Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string query = "INSERT INTO HRMEmployees (EmployeeCode, FirstName, LastName, FullName, Gender) VALUES (@EmployeeCode, @FirstName, @LastName, @FullName, @Gender)";

            try
            {
                bool? dbGender = null;
                if (gender == "Male") dbGender = true;
                else if (gender == "Female") dbGender = false;

                using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString))
                {
                    connection.Execute(query, new
                    {
                        EmployeeCode = code,
                        FirstName = first,
                        LastName = last,
                        FullName = full,
                        Gender = dbGender
                    });
                }
                MessageBox.Show("Employee record added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadEmployeeData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding employee:\n{ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteUpdate()
        {
            if (SelectedEmployee == null)
            {
                MessageBox.Show("Please select an employee record to update.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string code = EmployeeForm.EmployeeCode.Trim();
            string first = EmployeeForm.FirstName.Trim();
            string last = EmployeeForm.LastName.Trim();
            string full = EmployeeForm.FullName.Trim();
            string gender = EmployeeForm.Gender;

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(first) || string.IsNullOrEmpty(last))
            {
                MessageBox.Show("Please fill Employee Code, First Name, and Last Name.", "Validation Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string query = "UPDATE HRMEmployees SET EmployeeCode = @EmployeeCode, FirstName = @FirstName, LastName = @LastName, FullName = @FullName, Gender = @Gender WHERE Id = @Id";

            try
            {
                bool? dbGender = null;
                if (gender == "Male") dbGender = true;
                else if (gender == "Female") dbGender = false;

                using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString))
                {
                    connection.Execute(query, new
                    {
                        Id = SelectedEmployee.Id,
                        EmployeeCode = code,
                        FirstName = first,
                        LastName = last,
                        FullName = full,
                        Gender = dbGender
                    });
                }
                MessageBox.Show("Employee record updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadEmployeeData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating employee:\n{ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDelete()
        {
            if (SelectedEmployee == null)
            {
                MessageBox.Show("Please select an employee record to delete.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult confirm = MessageBox.Show("Are you sure you want to delete this employee record?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                string query = "DELETE FROM HRMEmployees WHERE Id = @Id";

                try
                {
                    using (SqlConnection connection = new SqlConnection(AppConfig.ConnectionString))
                    {
                        connection.Execute(query, new { Id = SelectedEmployee.Id });
                    }
                    MessageBox.Show("Employee record deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadEmployeeData();
                    ClearInputs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting employee:\n{ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExecuteLogout()
        {
            _windowService.ShowLogin();
        }
    }
}
