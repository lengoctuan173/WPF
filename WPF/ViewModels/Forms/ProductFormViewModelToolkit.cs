using System;
using CommunityToolkit.Mvvm.ComponentModel;
using WPF.Helpers;

namespace WPF.ViewModels.Forms
{
    public partial class ProductFormViewModelToolkit : ObservableObject
    {
        [ObservableProperty]
        private string _productCode = string.Empty;

        [ObservableProperty]
        private string _productName = string.Empty;

        private string _priceText = "0";
        private string _quantityText = "0";

        public string PriceText
        {
            get => _priceText;
            set => SetProperty(ref _priceText, NumberHelper.FormatDecimal(value));
        }

        public string QuantityText
        {
            get => _quantityText;
            set => SetProperty(ref _quantityText, NumberHelper.FormatInt(value));
        }

        public void Clear()
        {
            ProductCode = string.Empty;
            ProductName = string.Empty;
            PriceText = "0";
            QuantityText = "0";
        }
    }
}
