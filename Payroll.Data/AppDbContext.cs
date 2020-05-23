using Microsoft.EntityFrameworkCore;
using Payroll.Data.Abstractions;
using Payroll.Data.Models;
using Payroll.Shared.Statics;
using System.Threading.Tasks;

namespace Payroll.Data
{
    public class AppDbContext : DbContext, IDbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Models.Payroll> Payrolls { get; set; }

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionStrings.MySqlConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Payroll>()
               .HasOne(a => a.Employee)
               .WithMany(b => b.Payrolls)
               .HasForeignKey(p => p.EmployeeID)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
