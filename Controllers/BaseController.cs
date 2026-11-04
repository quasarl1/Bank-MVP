using System.Security.Claims;
using SystemBank;
using Microsoft.AspNetCore.Mvc;

namespace SystemBank.Controllers;
[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    // protected string? GetUserID() => User?.FindFirst(с => с.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("error in id");
}