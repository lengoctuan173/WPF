using System;

namespace WPF.ViewModels.Forms
{
    public class EmployeeFormViewModel : ViewModelBase
    {
        private string _employeeCode = string.Empty;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _fullName = string.Empty;
        private string _gender = string.Empty;

        public string EmployeeCode
        {
            get => _employeeCode;
            set => SetProperty(ref _employeeCode, value);
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (SetProperty(ref _firstName, value))
                {
                    UpdateFullName();
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (SetProperty(ref _lastName, value))
                {
                    UpdateFullName();
                }
            }
        }

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        private void UpdateFullName()
        {
            FullName = $"{LastName.Trim()} {FirstName.Trim()}".Trim();
        }

        public void Clear()
        {
            EmployeeCode = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            FullName = string.Empty;
            Gender = string.Empty;
        }
    }
}
