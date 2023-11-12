using Microsoft.AspNetCore.Mvc;

namespace FVStorage.Controllers;

[Route("api")]
[ApiController]
public class DiscoveryEndpointController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() {
        var welcome = new {
            _links = new {
                vehicles = new {
                    href = "/api/supplies"
                }
            },
            message = "Welcome to the FVStorage API!",
        };
        return Ok(welcome);
    }
}