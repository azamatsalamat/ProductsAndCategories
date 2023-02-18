using Microsoft.EntityFrameworkCore;
using ProductsAndCategoriesLibrary;

namespace ProductsAndCategoriesAPI
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<ProductProperty> ProductProperties { get; set; }
        public DbSet<ProductFile> ProductImages { get; set; }

        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProductsAndCategoriesDB;Trusted_Connection=True;");
        }
    }
}
