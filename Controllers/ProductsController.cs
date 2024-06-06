using CustomerManagementSystem.Models;
using CustomerManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] Product product)
        {
            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.ProductId) return BadRequest();
            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
