using Microsoft.AspNetCore.Mvc;
using SystemBank.Generics;
using SystemBank.Features;
using SystemBank.Models;
using SystemBank.Middleware;
using Microsoft.VisualBasic;
using MediatR;
namespace SystemBank.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ControllerTransaction : BaseController
{
    private readonly IMediator mediator;
    public ControllerTransaction(IMediator _mediator) => mediator = _mediator;

    [HttpPost("Transaction/Transfer")]
    public async Task<IActionResult> Transaction([FromBody] TransactionRecord transaction)
    {
        BaseResponse<UserTransactionsDto> FirstcommandResult = await mediator.Send(transaction);
        return FirstcommandResult.Success ? Ok(FirstcommandResult) : BadRequest(FirstcommandResult);
    }
}