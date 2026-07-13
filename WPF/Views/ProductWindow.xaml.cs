using System.Windows;
using WPF.ViewModels;

namespace WPF.Views
{
    public partial class ProductWindow : Window
    {
        public ProductWindow(ProductViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
