using FVStorage.Models;
using FVStorage.Entities;

using Microsoft.AspNetCore.Mvc;

namespace FVStorage.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IFVStorageStorage _db;

    public ProductsController(IFVStorageStorage db)
    {
        this._db = db;
    }

    [HttpGet]
    public IEnumerable<Product> Get()
    {
        return _db.ListProducts();
    }

    [HttpGet("{code}")]
    public IActionResult Get(string code)
    {
        var productModel = _db.FindProduct(code);
        if (productModel == default) return NotFound();
        var resource = productModel.ToDynamic();
        resource._actions = new
        {
            delete = new
            {
                href = $"/api/products/{code}",
                method = "DELETE",
                name = $"Delete {code} from the database"
            },
            put = new
            {
                href = $"/api/products/{code}",
                method = "PUT",
                name = $"Put {code} in the database"
            }
        };
        return Ok(resource);
    }
    
    [HttpPut("{code}")]
    public IActionResult Put(string code, [FromBody] ProductDTO dto)
    {
        var sFindProduct = _db.FindProduct(code);
        var product = new Product()
        {
            Code = code,
            Name = dto.Name,
            Weight = dto.Weight,
            Price = dto.Price,
            SupplierCode = dto.SupplierCode
        };
        _db.UpdateProduct(product);
        return Ok(dto);
    }
    [HttpPost("{code}")]
    public async Task<IActionResult> Post(string code,[FromBody] ProductDTO dto)
    {
        var supplier = _db.FindSupplier(dto.SupplierCode);
        var product = new Product
        {
            Code = code,
            Name = dto.Name,
            Weight = dto.Weight,
            Price = dto.Price,
            SupplierCode = dto.SupplierCode,
            Supplier = supplier
        };
        _db.CreateProduct(product);
        // await PublishNewSupplyMessage(supply);
        return Created($"/api/vehicles/{product.Code}", product.ToResourceProduct());
    }
    [HttpDelete("{code}")]
    public IActionResult Delete(string code)
    {
        var product = _db.FindProduct(code);
        if (product == null) return NotFound();
        _db.DeleteProduct(product);
        return NoContent();
    }
    
    
}