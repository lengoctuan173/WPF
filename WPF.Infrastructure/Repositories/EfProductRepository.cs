using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WPF.Domain.Models;
using WPF.Domain.Repositories;

namespace WPF.Infrastructure.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        private readonly IDbContextFactory<WPFDbContext> _contextFactory;

        public EfProductRepository(IDbContextFactory<WPFDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<int> AddProductAsync(Product product)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Products.Add(product);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateProductAsync(Product product)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var existing = await context.Products.FindAsync(product.Id);
            if (existing != null)
            {
                context.Entry(existing).CurrentValues.SetValues(product);
                context.Entry(existing).Property(p => p.CreatedDate).IsModified = false;
                context.Entry(existing).Property(p => p.CreatedBy).IsModified = false;
                return await context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var product = await context.Products.FindAsync(id);
            if (product != null)
            {
                context.Products.Remove(product);
                return await context.SaveChangesAsync();
            }
            return 0;
        }
    }
}
