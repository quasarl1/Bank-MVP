using Microsoft.AspNetCore.Mvc;
using SystemBank.Controllers;
using SystemBank.Generics;
using SystemBank.Models;

namespace SystemBank;
[ApiController]
[Route("api/[controller]")]
public class ControllerDelete : BaseController 
{
    private readonly DeleteAccount delete;
    public ControllerDelete(DeleteAccount _delete)
    {
        delete = _delete;
    }

    [HttpDelete("account/delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await delete.Delete(id);
        return Ok(result.Data);
    }
    }