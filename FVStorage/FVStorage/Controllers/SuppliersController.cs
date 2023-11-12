using FVStorage.Models;
using FVStorage.Entities;

using Microsoft.AspNetCore.Mvc;

namespace FVStorage.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuppliersController : ControllerBase
{
    private readonly IFVStorageStorage _db;

    public SuppliersController(IFVStorageStorage db)
    {
        this._db = db;
    }

    [HttpGet]
    public IEnumerable<Supplier> Get()
    {
        return _db.ListSuppliers();
    }

    [HttpGet("{code}")]
    public IActionResult Get(string code)
    {
        var supplier = _db.FindSupplier(code);
        if (supplier == default) return NotFound();
        var resource = supplier.ToDynamic();
        resource._actions = new
        {
            delete = new
            {
                href = $"/api/suppliers/{code}",
                method = "DELETE",
                name = $"Delete {code} from the database"
            },
            put = new
            {
                href = $"/api/suppliers/{code}",
                method = "PUT",
                name = $"Put {code} in the database"
            }
        };
        return Ok(resource);
    }
    
    [HttpPut("{code}")]
    public IActionResult Put(string code, [FromBody] SupplierDto dto)
    {
        var supplier = new Supplier
        {
            Code = code,
            Name = dto.Name,
        };
        _db.UpdateSupplier(supplier);
        return Ok(dto);
    }
    [HttpPost("{code}")]
    public async Task<IActionResult> Post(string code,[FromBody] SupplierDto dto)
    {
        var supplier = new Supplier
        {
            Code = code,
            Name = dto.Name,
        };
        _db.CreateSupplier(supplier);
        // await PublishNewSupplyMessage(supply);
        return Created($"/api/suppliers/{supplier.Code}", supplier.ToResourceSupplier());
    }
    [HttpDelete("{code}")]
    public IActionResult Delete(string code)
    {
        var supplier = _db.FindSupplier(code);
        if (supplier == null) return NotFound();
        _db.DeleteSupplier(supplier);
        return NoContent();
    }
    
    
}
