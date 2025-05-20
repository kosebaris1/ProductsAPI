using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.DTO;
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
            var products = await _context
                .Products
                .Where(i => i.IsActive)
                .Select(p =>  ProductToDTO(p))
                .ToListAsync();
            return Ok(products);
        }

        [Authorize]
        [HttpGet("api/[controller]/{id}")]  // [httpGet("{id}")
        public async Task<IActionResult> GetProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = await _context
                .Products
                .Where(i => i.ProductId == id)
                .Select(p =>ProductToDTO(p))
                .FirstOrDefaultAsync(i => i.ProductId == id);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)
        {
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = entity.ProductId }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product entity)
        {
            if (id != entity.ProductId)
            {
                return BadRequest();
            }
            var result = await _context.Products.FirstOrDefaultAsync(i => i.ProductId == id);

            if(result == null)
            {
                return NotFound();
            }

            result.ProductName = entity.ProductName;
            result.Price = entity.Price;
            result.IsActive = entity.IsActive;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if( id == null)
            {
                return NotFound();
            }

             var result = await _context.Products.FirstOrDefaultAsync(i => i.ProductId == id);

            if (result == null)
            {
                return NotFound();
            }

            _context.Products.Remove(result);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent() ;
        }

        private static ProductDTO ProductToDTO(Product product)
        {
            var entity= new ProductDTO();
            if(product != null)
            {
                entity.ProductId = product.ProductId;   
                entity.ProductName = product.ProductName;   
                entity.Price = product.Price;   
            }
            return new ProductDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
            };
        }
    }
}
