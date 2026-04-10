using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace PizzaDelivery.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Health check endpoint
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            service = "Pizza Delivery API"
        });
    }
}

/// <summary>
/// Root health check
/// </summary>
[ApiController]
[Route("health")]
public class RootHealthController : ControllerBase
{
    /// <summary>
    /// Root health endpoint for Docker
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Get()
    {
        return Ok("OK");
    }
}
