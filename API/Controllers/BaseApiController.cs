using API.Errors;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        return result.StatusCode switch
        {
            200 => Ok(result.Data),
            204 => NoContent(),
            401 => Unauthorized(result.Error),
            404 => NotFound(result.Error),
            _ => BadRequest(result.Error)
        };
    }
}