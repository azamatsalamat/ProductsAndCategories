using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsAndCategoriesLibrary;

namespace ProductsAndCategoriesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("All")]
        public async Task<ActionResult<List<Category>>> GetAll()
        {
            return await Services.CategoryService.GetAllCategories(_db);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            try
            {
                return await Services.CategoryService.GetCategory(_db, id);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("properties/{categoryId}")]
        public async Task<ActionResult<List<Property>>> GetAllPropertiesForCategory(int categoryId)
        {
            try
            {
                return await Services.CategoryService.GetAllPropertiesForCategory(_db, categoryId);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("properties/{categoryId}/name/{name}")]
        public async Task<ActionResult<Property>> GetPropertyByName(int categoryId, string name)
        {
            try
            {
                return await Services.CategoryService.GetPropertyByName(_db, name, categoryId);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{name}")]
        public async Task<ActionResult<Category>> AddCategory(string name)
        {
            try
            {
                return await Services.CategoryService.AddCategory(_db, name);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("property/{categoryId}/{name}")]
        public async Task<ActionResult<Property>> AddPropertyToCategory(int categoryId, string name, string type)
        {
            try
            {
                return await Services.CategoryService.AddPropertyToCategory(_db, categoryId, name, type);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Category>>> DeleteCategory(int id)
        {
            try
            {
                return await Services.CategoryService.DeleteCategory(_db, id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
