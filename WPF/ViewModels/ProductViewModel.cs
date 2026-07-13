using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPF.Domain.Models;
using WPF.Application.Services;
using WPF.Services;
using WPF.ViewModels.Forms;
using WPF.Helpers;

namespace WPF.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        private ObservableCollection<Product> _products = new();
        private Product? _selectedProduct;
        
        private string _statusMessage = "Connection status: Ready OK";
        private Brush _statusColor = Brushes.Gray;

        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (SetProperty(ref _selectedProduct, value))
                {
                    OnSelectedProductChanged();
                }
            }
        }

        public ProductFormViewModel ProductForm { get; } = new();

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
        private readonly IProductService _productService;

        public ProductViewModel(IWindowService windowService, IProductService productService)
        {
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));

            AddCommand = new RelayCommand(async () => await ExecuteAddAsync());
            UpdateCommand = new RelayCommand(async () => await ExecuteUpdateAsync());
            DeleteCommand = new RelayCommand(async () => await ExecuteDeleteAsync());
            LogoutCommand = new RelayCommand(ExecuteLogout);

            _ = LoadProductDataAsync();
        }

        public async Task LoadProductDataAsync()
        {
            try
            {
                StatusMessage = "Loading product list...";
                var list = await _productService.GetAllProductsAsync();
                Products = new ObservableCollection<Product>(list);
                StatusMessage = "Data loaded successfully from database.";
                StatusColor = Brushes.Gray;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product data:\n{ex.Message}", "Database Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusMessage = "Failed to load product list. Check database connection settings.";
                StatusColor = Brushes.Red;
            }
        }

        private void OnSelectedProductChanged()
        {
            if (SelectedProduct != null)
            {
                ProductForm.ProductCode = SelectedProduct.ProductCode;
                ProductForm.ProductName = SelectedProduct.ProductName;
                ProductForm.PriceText = SelectedProduct.Price.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                ProductForm.QuantityText = SelectedProduct.Quantity.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
            }
            else
            {
                ClearInputs();
            }
        }

        private void ClearInputs()
        {
            ProductForm.Clear();
        }

        private async Task ExecuteAddAsync()
        {
            string code = ProductForm.ProductCode.Trim();
            string name = ProductForm.ProductName.Trim();
            string priceStr = ProductForm.PriceText.Trim();
            string quantityStr = ProductForm.QuantityText.Trim();

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please fill Product Code and Product Name.", "Validation Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!NumberHelper.TryParseDecimal(priceStr, out decimal price) || price < 0)
            {
                MessageBox.Show("Price must be a valid non-negative decimal number.", "Validation Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!NumberHelper.TryParseInt(quantityStr, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Quantity must be a valid non-negative integer.", "Validation Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newProduct = new Product
            {
                ProductCode = code,
                ProductName = name,
                Price = price,
                Quantity = quantity
            };

            try
            {
                bool success = await _productService.AddProductAsync(newProduct);
                if (success)
                {
                    MessageBox.Show("Product record added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadProductDataAsync();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Failed to add product record.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product:\n{ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ExecuteUpdateAsync()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Please select a product record to update.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string code = ProductForm.ProductCode.Trim();
            string name = ProductForm.ProductName.Trim();
            string priceStr = ProductForm.PriceText.Trim();
            string quantityStr = ProductForm.QuantityText.Trim();

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please fill Product Code and Product Name.", "Validation Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!NumberHelper.TryParseDecimal(priceStr, out decimal price) || price < 0)
            {
                MessageBox.Show("Price must be a valid non-negative decimal number.", "Validation Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!NumberHelper.TryParseInt(quantityStr, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Quantity must be a valid non-negative integer.", "Validation Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updatedProduct = new Product
            {
                Id = SelectedProduct.Id,
                ProductCode = code,
                ProductName = name,
                Price = price,
                Quantity = quantity
            };

            try
            {
                bool success = await _productService.UpdateProductAsync(updatedProduct);
                if (success)
                {
                    MessageBox.Show("Product record updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadProductDataAsync();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Failed to update product record.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product:\n{ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ExecuteDeleteAsync()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Please select a product record to delete.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult confirm = MessageBox.Show("Are you sure you want to delete this product record?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    bool success = await _productService.DeleteProductAsync(SelectedProduct.Id);
                    if (success)
                    {
                        MessageBox.Show("Product record deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadProductDataAsync();
                        ClearInputs();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete product record.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting product:\n{ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExecuteLogout()
        {
            _windowService.ShowLogin();
        }
    }
}
