using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPF.Domain.Models;
using WPF.Domain.Repositories;

namespace WPF.Infrastructure.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public MockProductRepository()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, ProductCode = "PROD001", ProductName = "iPhone 15 Pro", Price = 999.99m, Quantity = 50, CreatedDate = DateTime.Now, CreatedBy = "admin" },
                new Product { Id = 2, ProductCode = "PROD002", ProductName = "Samsung Galaxy S24", Price = 899.99m, Quantity = 40, CreatedDate = DateTime.Now, CreatedBy = "admin" },
                new Product { Id = 3, ProductCode = "PROD003", ProductName = "MacBook Air M3", Price = 1099.00m, Quantity = 25, CreatedDate = DateTime.Now, CreatedBy = "admin" }
            };
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return Task.FromResult<IEnumerable<Product>>(_products.ToList());
        }

        public Task<int> AddProductAsync(Product product)
        {
            int newId = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            product.Id = newId;
            product.CreatedDate = DateTime.Now;
            product.CreatedBy = "admin";
            _products.Add(product);
            return Task.FromResult(1);
        }

        public Task<int> UpdateProductAsync(Product product)
        {
            var existing = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existing != null)
            {
                existing.ProductCode = product.ProductCode;
                existing.ProductName = product.ProductName;
                existing.Price = product.Price;
                existing.Quantity = product.Quantity;
                existing.UpdatedDate = DateTime.Now;
                existing.UpdatedBy = product.UpdatedBy ?? "admin";
                return Task.FromResult(1);
            }
            return Task.FromResult(0);
        }

        public Task<int> DeleteProductAsync(int id)
        {
            var existing = _products.FirstOrDefault(p => p.Id == id);
            if (existing != null)
            {
                _products.Remove(existing);
                return Task.FromResult(1);
            }
            return Task.FromResult(0);
        }
    }
}
