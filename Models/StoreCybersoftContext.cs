//ef-dbcontext-ip
using Microsoft.EntityFrameworkCore;

namespace webapi_blazor.Models {
    public class StoreCybersoftContext : DbContext {
        public StoreCybersoftContext() { }
        public StoreCybersoftContext(DbContextOptions<StoreCybersoftContext> options) : base (options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=StoreCybersoft;User Id=sa;Password=khaicybersoft2109@;TrustServerCertificate=True;");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}