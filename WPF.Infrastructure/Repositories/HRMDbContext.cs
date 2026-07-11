using Microsoft.EntityFrameworkCore;
using WPF.Domain.Models;

namespace WPF.Infrastructure.Repositories
{
    public class HRMDbContext : DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();

        public HRMDbContext(DbContextOptions<HRMDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("HRMEmployeesTest");
                entity.HasKey(e => e.Id);

                // Cấu hình FullName là cột computed (tự sinh bởi DB, không được insert/update từ EF Core)
                entity.Property(e => e.FullName)
                    .ValueGeneratedOnAddOrUpdate();

            });
        }
    }
}
