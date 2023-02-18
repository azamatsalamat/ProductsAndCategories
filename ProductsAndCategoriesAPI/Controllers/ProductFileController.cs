using Microsoft.AspNetCore.Mvc;
using ProductsAndCategoriesLibrary;

namespace ProductsAndCategoriesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductFileController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductFileController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost("image/{productId}")]
        public async Task<ActionResult<ProductFile>> AddProductImage(int productId, IFormFile file)
        {
            try
            {
                return await Services.ProductService.AddProductImage(_db, productId, file);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("image/{productId}")]
        public async Task<ActionResult<List<ProductFile>>> GetFilesForProduct(int productId)
        {
            try
            {
                return await Services.ProductService.GetFilesForProduct(_db, productId);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("image/{fileId}")]
        public async Task<ActionResult<List<ProductFile>>> DeleteFile(int fileId)
        {
            try
            {
                return await Services.ProductService.DeleteFile(_db, fileId);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
