using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPF.Models;
using WPF.Services;
using WPF.ViewModels.Forms;

namespace WPF.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
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
        private readonly IEmployeeService _employeeService;

        public EmployeeViewModel(IWindowService windowService, IEmployeeService employeeService)
        {
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));

            AddCommand = new RelayCommand(async () => await ExecuteAddAsync());
            UpdateCommand = new RelayCommand(async () => await ExecuteUpdateAsync());
            DeleteCommand = new RelayCommand(async () => await ExecuteDeleteAsync());
            LogoutCommand = new RelayCommand(ExecuteLogout);

            _ = LoadEmployeeDataAsync();
        }

        public async Task LoadEmployeeDataAsync()
        {
            try
            {
                StatusMessage = "Loading employee list...";
                var list = await _employeeService.GetAllEmployeesAsync();
                Employees = new ObservableCollection<Employee>(list);
                StatusMessage = "Data loaded successfully from HRM database.";
                StatusColor = Brushes.Gray;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employee data:\n{ex.Message}", "Database Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                EmployeeForm.FullName = SelectedEmployee.FullName;
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

        private async Task ExecuteAddAsync()
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

            var newEmployee = new Employee
            {
                EmployeeCode = code,
                FirstName = first,
                LastName = last,
                FullName = full,
                Gender = gender
            };

            try
            {
                bool success = await _employeeService.AddEmployeeAsync(newEmployee);
                if (success)
                {
                    MessageBox.Show("Employee record added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadEmployeeDataAsync();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Failed to add employee record.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding employee:\n{ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ExecuteUpdateAsync()
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

            var updatedEmployee = new Employee
            {
                Id = SelectedEmployee.Id,
                EmployeeCode = code,
                FirstName = first,
                LastName = last,
                FullName = full,
                Gender = gender
            };

            try
            {
                bool success = await _employeeService.UpdateEmployeeAsync(updatedEmployee);
                if (success)
                {
                    MessageBox.Show("Employee record updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadEmployeeDataAsync();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Failed to update employee record.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating employee:\n{ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ExecuteDeleteAsync()
        {
            if (SelectedEmployee == null)
            {
                MessageBox.Show("Please select an employee record to delete.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult confirm = MessageBox.Show("Are you sure you want to delete this employee record?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    bool success = await _employeeService.DeleteEmployeeAsync(SelectedEmployee.Id);
                    if (success)
                    {
                        MessageBox.Show("Employee record deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadEmployeeDataAsync();
                        ClearInputs();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete employee record.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
