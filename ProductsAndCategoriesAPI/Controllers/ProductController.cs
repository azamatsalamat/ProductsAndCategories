using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsAndCategoriesLibrary;
using Microsoft.AspNetCore.Hosting;

namespace ProductsAndCategoriesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost("{categoryId}/{name}/{price}")]
        public async Task<ActionResult<Product>> AddProduct(int categoryId, string name, string description, double price, Dictionary<string, string> propertyValues)
        {
            try
            {
                return await Services.ProductService.AddProduct(_db, categoryId, name, description, price, propertyValues);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                return await Services.ProductService.GetProduct(_db, id);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("productProperty/{id}")]
        public async Task<ActionResult<List<ProductProperty>>> GetPropertiesForProduct(int id)
        {
            try
            {
                return await Services.ProductService.GetPropertiesForProduct(_db, id);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("All")]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            return await Services.ProductService.GetAllProducts(_db);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<List<Product>>> GetAllProductsForCategory(int categoryId)
        {
            try
            {
                return await Services.ProductService.GetAllProductsForCategory(_db, categoryId);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("sort/{categoryId}/name")]
        public async Task<ActionResult<List<Product>>> SortProductsByName(int categoryId, bool isAscending)
        {
            try
            {
                return await Services.ProductService.SortProductsByName(_db, categoryId, isAscending);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("sort/{categoryId}/price")]
        public async Task<ActionResult<List<Product>>> SortProductsByPrice(int categoryId, bool isAscending)
        {
            try
            {
                return await Services.ProductService.SortProductsByPrice(_db, categoryId, isAscending);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("sort/{categoryId}/{propertyName}")]
        public async Task<ActionResult<List<Product>>> SortProductsByProperty(int categoryId, string propertyName, bool isAscending)
        {
            try
            {
                return await Services.ProductService.SortProductsByProperty(_db, categoryId, propertyName, isAscending);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("sort/{categoryId}/{propertyName}/{propertyValue}")]
        public async Task<ActionResult<List<Product>>> GetProductsByPropertyValue(int categoryId, string propertyName, string propertyValue)
        {
            try
            {
                return await Services.ProductService.GetProductsByPropertyValue(_db, categoryId, propertyName, propertyValue);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        
    }
}
