using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsContext _context;

        public ProductsController(ProductsContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products =await _context.Products.ToListAsync(); 
            return Ok(products);
        }

        [HttpGet("api/[controller]/{id}")]  // [httpGet("{id}")
        public async Task<IActionResult> GetProduct(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var result =await _context.Products.FirstOrDefaultAsync(i => i.ProductId == id);   

            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}
