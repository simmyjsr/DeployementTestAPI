using InventoryAPI.Models;
using InventoryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _service;

        public SupplierController(ISupplierService service)
        {
            _service = service;
        }

        [HttpGet ("GetAllActiveSupplier")]

        public async Task <ActionResult<IEnumerable<Supplier>>>GetAll() => Ok(await _service.GetAllSupplier());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var supplier = _service.GetSupplierById(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        [HttpPost("CreateSupplier")]
        public IActionResult Create([FromBody] Supplier supplier)
        {
            _service.AddSupplier(supplier);
            return Ok();
        }

        [HttpPut("UpdateSupplier")]
        public IActionResult Update([FromBody] Supplier supplier)
        {
            _service.UpdateSupplier(supplier);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.DeleteSupplier(id);
            return Ok();
        }
    }
}
