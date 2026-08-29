using Microsoft.AspNetCore.Mvc;
using webapikits.Service;

namespace webapikits.Controllers;

[ApiController]
[Route("pbx")]
public class PbxGatewayController : ControllerBase
{
    private readonly PbxGatewayHealthService _healthService;

    public PbxGatewayController(PbxGatewayHealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet("health")]
    public async Task<IActionResult> Health()
    {
        var result = await _healthService.GetHealthAsync();
        return Ok(result);
    }
}