using System.Windows;
using WPF.ViewModels;

namespace WPF.Views
{
    public partial class EmployeeWindow : Window
    {
        public EmployeeWindow(EmployeeViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
