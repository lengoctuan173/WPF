using System;
using System.Windows;
using WPF.ViewModels;

namespace WPF.Views
{
    public partial class HomeWindow : Window
    {
        public HomeWindow(HomeViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void dgvEmployees_SelectionChanged()
        {

        }
    }
}
