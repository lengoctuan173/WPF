using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using WPF.Domain.Models;
using WPF.Domain.Repositories;

namespace WPF.Infrastructure.Repositories
{
    public class DapperProductRepository : IProductRepository
    {
        private readonly Func<IDbConnection> _connectionFactory;

        public DapperProductRepository(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            const string query = "SELECT Id, ProductCode, ProductName, Price, Quantity, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM Products";

            using var connection = _connectionFactory();
            return await connection.QueryAsync<Product>(query);
        }

        public async Task<int> AddProductAsync(Product product)
        {
            const string query = @"
                INSERT INTO Products (ProductCode, ProductName, Price, Quantity, CreatedBy) 
                VALUES (@ProductCode, @ProductName, @Price, @Quantity, @CreatedBy)";

            using var connection = _connectionFactory();
            return await connection.ExecuteAsync(query, new
            {
                product.ProductCode,
                product.ProductName,
                product.Price,
                product.Quantity,
                product.CreatedBy
            });
        }

        public async Task<int> UpdateProductAsync(Product product)
        {
            const string query = @"
                UPDATE Products 
                SET ProductCode = @ProductCode, 
                    ProductName = @ProductName, 
                    Price = @Price, 
                    Quantity = @Quantity, 
                    UpdatedDate = SYSDATETIME(), 
                    UpdatedBy = @UpdatedBy 
                WHERE Id = @Id";

            using var connection = _connectionFactory();
            return await connection.ExecuteAsync(query, new
            {
                product.Id,
                product.ProductCode,
                product.ProductName,
                product.Price,
                product.Quantity,
                product.UpdatedBy
            });
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            const string query = "DELETE FROM Products WHERE Id = @Id";

            using var connection = _connectionFactory();
            return await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
