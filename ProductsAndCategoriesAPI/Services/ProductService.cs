using Microsoft.EntityFrameworkCore;
using ProductsAndCategoriesLibrary;

namespace ProductsAndCategoriesAPI.Services
{
    public static class ProductService
    {
        public static async Task<Product> AddProduct(ApplicationDbContext db, int categoryId, string name, string description, double price, Dictionary<string, string> propertyValues)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price));
            }

            var category = await CategoryService.GetCategory(db, categoryId);

            var product = new Product { Name = name, Category = category, Description = description, Price = price, CategoryId = categoryId };

            var productProperties = new List<ProductProperty>();
            foreach (var propertyValue in propertyValues)
            {
                var property = await CategoryService.GetPropertyByName(db, propertyValue.Key, categoryId);
                if (!category.Properties.Select(p => p.Id).Contains(property.Id))
                {
                    throw new ArgumentException("Invalid property", nameof(propertyValues));
                }

                if (property.Type.ToLower() == "numeric")
                {
                    Convert.ToDouble(propertyValue.Value);
                }

                var productProperty = new ProductProperty { Product = product, Property = property, Value = propertyValue.Value, ProductId = product.Id, PropertyId = property.Id };
                productProperties.Add(productProperty);
            }

            await db.Products.AddAsync(product);
            await db.SaveChangesAsync();
            await db.ProductProperties.AddRangeAsync(productProperties);
            await db.SaveChangesAsync();

            return product;
        }

        public static async Task<Product> GetProduct(ApplicationDbContext db, int productId)
        {
            var product = await db.Products.FindAsync(productId);
            if (product == null)
            {
                throw new ArgumentException("Cannot find product", nameof(productId));
            }
            return product;
        }

        public static async Task<List<ProductProperty>> GetPropertiesForProduct(ApplicationDbContext db, int productId)
        {
            var productProperties = await db.ProductProperties.Where(x => x.ProductId == productId).ToListAsync();

            if (productProperties == null)
            {
                throw new ArgumentException("Cannot find product", nameof(productId));
            }

            return productProperties;
        }

        public static async Task<List<Product>> GetAllProducts(ApplicationDbContext db)
        {
            return await db.Products.ToListAsync();
        }

        public static async Task<List<Product>> GetAllProductsForCategory(ApplicationDbContext db, int categoryId)
        {
            var products = await db.Products.Where(x => x.CategoryId == categoryId).ToListAsync();

            if (products == null)
            {
                throw new ArgumentException("Cannot find category", nameof(categoryId));
            }

            return products;
        }

        public static async Task<List<Product>> SortProductsByName(ApplicationDbContext db, int categoryId, bool isAscending = true)
        {
            var products = await GetAllProductsForCategory(db, categoryId);
            if (isAscending)
            {
                return products.OrderBy(x => x.Name).ToList();
            }
            else
            {
                return products.OrderByDescending(x => x.Name).ToList();
            }
        }

        public static async Task<List<Product>> SortProductsByPrice(ApplicationDbContext db, int categoryId, bool isAscending = true)
        {
            var products = await GetAllProductsForCategory(db, categoryId);
            if (isAscending)
            {
                return products.OrderBy(x => x.Price).ToList();
            }
            else
            {
                return products.OrderByDescending(x => x.Price).ToList();
            }
        }

        public static async Task<List<Product>> SortProductsByProperty(ApplicationDbContext db, int categoryId, string propertyName, bool isAscending = true)
        {
            var property = await CategoryService.GetPropertyByName(db, propertyName, categoryId);
            var productProperties = db.ProductProperties.Where(x => x.Property == property).ToList();

            if (property.Type.ToLower() == "numeric")
            {
                if (isAscending)
                {
                    productProperties = productProperties.OrderBy(x => Convert.ToDouble(x.Value)).ToList();
                }
                else
                {
                    productProperties = productProperties.OrderByDescending(x => Convert.ToDouble(x.Value)).ToList();
                }
            }
            else
            {
                if (isAscending)
                {
                    productProperties = productProperties.OrderBy(x => x.Value).ToList();
                }
                else
                {
                    productProperties = productProperties.OrderByDescending(x => x.Value).ToList();
                }
            }

            var productsSorted = new List<Product>();

            foreach (var prop in productProperties)
            {
                productsSorted.Add(await GetProduct(db, prop.ProductId));
            }

            return productsSorted;
        }

        public static async Task<List<Product>> GetProductsByPropertyValue(ApplicationDbContext db, int categoryId, string propertyName, string propertyValue)
        {
            var property = await CategoryService.GetPropertyByName(db, propertyName, categoryId);
            var productProperties = db.ProductProperties.Where(x => x.Property == property && x.Value == propertyValue).ToList();

            var productsSorted = new List<Product>();

            foreach (var prop in productProperties)
            {
                productsSorted.Add(await GetProduct(db, prop.ProductId));
            }

            return productsSorted;
        }

        public static async Task<ProductFile> AddProductImage(ApplicationDbContext db, int productId, IFormFile file)
        {
            var product = await GetProduct(db, productId);
            var storageName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var productImage = new ProductFile
            {
                Name = file.FileName,
                ProductId = product.Id,
                Product = product,
                StorageName = storageName,
                Url = @"https://localhost:7202/" + storageName
            };

            FileProcessor.SaveFile(file, storageName);

            await db.ProductImages.AddAsync(productImage);
            await db.SaveChangesAsync();

            return productImage;
        }

        public static async Task<List<ProductFile>> GetFilesForProduct(ApplicationDbContext db, int productId)
        {
            var product = await GetProduct(db, productId);
            return await db.ProductImages.Where(x => x.Product == product).ToListAsync();
        }

        public static async Task<List<ProductFile>> DeleteFile(ApplicationDbContext db, int fileId)
        {
            var file = await db.ProductImages.FindAsync(fileId);
            if (file == null)
            {
                throw new ArgumentException("Cannot find file", nameof(fileId));
            }

            db.ProductImages.Remove(file);
            await db.SaveChangesAsync();

            FileProcessor.DeleteFile(file);

            return await db.ProductImages.ToListAsync();
        }
    }
}
