using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPF.Domain.Models;
using WPF.Domain.Repositories;

namespace WPF.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return _productRepository.GetAllProductsAsync();
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            // Business Validation
            if (string.IsNullOrWhiteSpace(product.ProductCode)) return false;
            if (string.IsNullOrWhiteSpace(product.ProductName)) return false;
            if (product.Price < 0) return false;
            if (product.Quantity < 0) return false;

            product.CreatedDate = DateTime.Now;
            product.CreatedBy = "admin"; // Defaulting to admin logged in user

            int result = await _productRepository.AddProductAsync(product);
            return result > 0;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (product.Id <= 0) return false;

            // Business Validation
            if (string.IsNullOrWhiteSpace(product.ProductCode)) return false;
            if (string.IsNullOrWhiteSpace(product.ProductName)) return false;
            if (product.Price < 0) return false;
            if (product.Quantity < 0) return false;

            product.UpdatedDate = DateTime.Now;
            product.UpdatedBy = "admin"; // Defaulting to admin logged in user

            int result = await _productRepository.UpdateProductAsync(product);
            return result > 0;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            if (id <= 0) return false;

            int result = await _productRepository.DeleteProductAsync(id);
            return result > 0;
        }
    }
}
