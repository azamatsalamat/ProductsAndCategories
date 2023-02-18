using Microsoft.EntityFrameworkCore;
using ProductsAndCategoriesLibrary;

namespace ProductsAndCategoriesAPI.Services
{
    public static class CategoryService
    {
        public static async Task<Category> GetCategory(ApplicationDbContext db, int categoryId)
        {
            var category = await db.Categories.Include(c => c.Properties).FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
            {
                throw new ArgumentException("Cannot find category with such ID", nameof(categoryId));
            }
            return category;
        }

        public static async Task<List<Category>> GetAllCategories(ApplicationDbContext db)
        {
            return await db.Categories.ToListAsync();
        }

        public static async Task<List<Property>> GetAllPropertiesForCategory(ApplicationDbContext db, int categoryId)
        {
            var category = await GetCategory(db, categoryId);
            return category.Properties;
        }

        public static async Task<Category> AddCategory(ApplicationDbContext db, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var category = new Category { Name = name };
            await db.Categories.AddAsync(category);
            await db.SaveChangesAsync();

            return category;
        }

        public static async Task<Property> AddPropertyToCategory(ApplicationDbContext db, int categoryId, string propertyName, string propertyType)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (propertyType.ToLower() != "text" && propertyType.ToLower() != "numeric")
            {
                throw new ArgumentException("Property type can be either Text or Numeric", nameof(propertyType));
            }

            var category = await GetCategory(db, categoryId);

            if (category.Properties == null)
            {
                category.Properties = new List<Property>();
            }

            var property = new Property { Name = propertyName, Type = propertyType };
            category.Properties.Add(property);
            await db.Properties.AddAsync(property);
            await db.SaveChangesAsync();

            return property;
        }

        public static async Task<Property> GetPropertyByName(ApplicationDbContext db, string propertyName, int categoryId)
        {
            var properties = await GetAllPropertiesForCategory(db, categoryId);
            var property = properties.FirstOrDefault(p => p.Name == propertyName);
            if (property == null)
            {
                throw new ArgumentException("Invalid property", nameof(propertyName));
            }
            return property;
        }

        public static async Task<List<Category>> DeleteCategory(ApplicationDbContext db, int categoryId)
        {
            var category = await GetCategory(db, categoryId);
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return await db.Categories.ToListAsync();
        }
    }
}
