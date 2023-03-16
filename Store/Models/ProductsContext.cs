using Microsoft.EntityFrameworkCore;

namespace TodoList.Models
{
    public class ProductsContext : DbContext 
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Type> Types { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public ProductsContext(DbContextOptions<ProductsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
