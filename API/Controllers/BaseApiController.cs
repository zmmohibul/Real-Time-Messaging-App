using API.Errors;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    public IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.StatusCode == 200)
        {
            return Ok(result.Data);
        }
        
        if (result.StatusCode == 204)
        {
            return NoContent();
        }
        
        if (result.StatusCode == 401)
        {
            return Unauthorized(result.Error);
        }

        if (result.StatusCode == 404)
        {
            return NotFound(result.Error);
        }

        return BadRequest(result.Error);
    }
}