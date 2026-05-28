using InventoryAPI.Models;
using InventoryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

       
        [HttpGet("AllProduct")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts() => Ok(await _service.GetAllProductsAsync());

        [HttpGet("GetAllBrands")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetAllBrand() => Ok(await _service.GetAllBrand());
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProductRequest product)
        {
            try
            {
                var effectedRow = await _service.CreateProductAsync(product);
                //return CreatedAtAction(nameof(GetProduct), new { id }, product);
                if (effectedRow == 1) 
                return Ok(new { product.ProductName, message = "created", isError = false });
                else
                {
                    return Ok(new { product.ProductName, message = "Product creation failed.", isError = true });
                   
                }

            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                // SQL error 2627 = Unique constraint violation
                // SQL error 2601 = Cannot insert duplicate key row
                return Ok(new {  isError = true, message = "Duplicate data. The product " + product.ProductName + " already exists." });
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return StatusCode(500, new { isError = true, message = "An error occurred while creating the product.", error = ex.Message });
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(Guid id, [FromForm] viewProduct product)
        {
            if (id != product.ProductID) return BadRequest();
            var success = await _service.UpdateProductAsync(product);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id) => await _service.RemoveProductAsync(id) ? NoContent() : NotFound();

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts( string search)          
        {
            var products = await _service.SearchProductsAsync(search);
            return Ok(products);
        }
    }

}



