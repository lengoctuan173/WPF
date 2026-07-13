using Microsoft.EntityFrameworkCore;
using WPF.Domain.Models;

namespace WPF.Infrastructure.Repositories
{
    public class WPFDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        public WPFDbContext(DbContextOptions<WPFDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ProductCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
                entity.Property(e => e.Quantity).HasDefaultValue(0);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATETIME()").ValueGeneratedOnAdd();
                entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
                entity.Property(e => e.UpdatedDate);
                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            });
        }
    }
}
