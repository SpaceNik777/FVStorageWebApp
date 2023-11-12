using FVStorage.Models;
using FVStorage.Entities;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;

namespace FVStorage.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuppliesController : ControllerBase
{
    private readonly IFVStorageStorage _db;
    // private readonly IBus _bus;

    public SuppliesController(IFVStorageStorage db)
    {
        this._db = db;
        // this._bus = bus;
    }

    const int PAGE_SIZE = 25;

    // GET: api/supplies
    [HttpGet]
    [Produces("application/hal+json")]
    public IActionResult Get(int index = 0, int count = PAGE_SIZE)
    {
        var items = _db.ListSupplies().Skip(index).Take(count)
            .Select(v => v.ToResourceSupply());
        var total = _db.CountSupplies();
        var _links = HAL.PaginateAsDynamic("/api/supplies", index, count, total);
        var result = new
        {
            _links,
            count,
            total,
            index,
            items
        };
        return Ok(result);
    }

    // GET api/supplies/id
    [HttpGet("{id}")]
    [Produces("application/hal+json")]
    public IActionResult Get(string id)
    {
        var supply = _db.FindSupply(id);
        if (supply == default) return NotFound();
        var resource = supply.ToResourceSupply();
        resource._actions = new
        {
            delete = new
            {
                href = $"/api/supplies/{id}",
                method = "DELETE",
                name = $"Delete {id} from the database"
            },
            put = new
            {
            href = $"/api/supplies/{id}",
            method = "PUT",
            name = $"Put {id} in the database"
        }
        };
        return Ok(resource);
    }

    // PUT api/supplies/id
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] SupplyDto dto)
    {
        var sFindSupply = _db.FindSupply(id);
        var supply = new Supply
        {
            Id = id,
            Date = dto.Date,
            Amount = dto.Amount,
            ProductCode = dto.ProductCode
        };
        _db.UpdateSupply(supply);
        return Ok(dto);
    }


    // POST api/supplies
    [HttpPost()]
    public async Task<IActionResult> Post([FromBody] SupplyDto dto)
    {
        var productModel = _db.FindProduct(dto.ProductCode);
        int supplyId = _db.CountSupplies() + 1;
        var supply = new Supply
        {
            Id = supplyId.ToString(),
            Date = dto.Date,
            Amount = dto.Amount,
            ProductModel = productModel
        };
        _db.CreateSupply(supply);
        // await PublishNewSupplyMessage(supply);
        return Created($"/api/vehicles/{supply.Id}", supply.ToResourceSupply());
    }

    // DELETE api/supplies/id
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var supply = _db.FindSupply(id);
        if (supply == null) return NotFound();
        _db.DeleteSupply(supply);
        return NoContent();
    }

    // private async Task PublishNewSupplyMessage(Supply supply) {
    //     var message = supply.ToMessage();
    //     await _bus.PubSub.PublishAsync(message);
    // }
}