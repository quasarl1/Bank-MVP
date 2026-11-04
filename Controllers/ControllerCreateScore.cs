using System;
using System.Net;
using SystemBank.Models;
using SystemBank.Middleware;
using SystemBank;
using SystemBank.Data;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SystemBank.Generics;
using SystemBank.Features;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
namespace SystemBank.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ControllerCreateScore : BaseController
{
    private readonly IMediator mediator;
    public ControllerCreateScore(IMediator _mediator)
    {
        mediator = _mediator;
    }
    [HttpPost("account/create-score")]
    public async Task<IActionResult> ControllerCreate(CreateUserRecord Createrecord)
    {
        var FirstcommandResult = Createrecord with {UserId = Createrecord.UserId, Type = Createrecord.Type};
        BaseResponse<AccountDTO> response = await mediator.Send(FirstcommandResult);
        return Ok(response);
    }
}