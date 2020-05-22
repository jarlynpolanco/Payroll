using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Payroll.Data.Abstractions;
using Payroll.Data.Models;
using Payroll.Shared.Models;
using System.Threading.Tasks;

namespace Payroll.Data
{
    public class AppDbContext : DbContext, IDbContext
    {
        private readonly IOptions<AppSettings> _appSettings;

        public AppDbContext(IOptions<AppSettings> appSettings) 
        {
            _appSettings = appSettings;
        }

        public AppDbContext()
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Models.Payroll> Payrolls { get; set; }

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=UNAPEC;user=root;password=root");
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
