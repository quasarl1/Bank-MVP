using Microsoft.AspNetCore.Mvc;
using SystemBank.Generics;
using SystemBank.Features;
using SystemBank.Models;
using SystemBank.Middleware;
using Microsoft.VisualBasic;
namespace SystemBank.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ControllerGetScore : BaseController 
{
    private readonly GetUser getuser;
    public ControllerGetScore(GetUser _getuser)
    {
        getuser = _getuser;
    }
    [HttpGet("get/accounts")]
    public async Task<IActionResult> Get(string id)
    {
        var account = await getuser.Get(id);
        return Ok(account.Data);
    }
    }