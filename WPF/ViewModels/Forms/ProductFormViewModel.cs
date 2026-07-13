using System;
using WPF.Helpers;

namespace WPF.ViewModels.Forms
{
    public class ProductFormViewModel : ViewModelBase
    {
        private string _productCode = string.Empty;
        private string _productName = string.Empty;
        private string _priceText = "0";
        private string _quantityText = "0";

        public string ProductCode
        {
            get => _productCode;
            set => SetProperty(ref _productCode, value);
        }

        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value);
        }

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
