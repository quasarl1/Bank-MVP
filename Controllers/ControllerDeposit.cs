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
public class DepositeController : BaseController
{
    private readonly IMediator mediator;
    public DepositeController(IMediator _mediator) => mediator = _mediator;
    [HttpPost("account/deposite")]
     public async Task<BaseResponse<UserTransactionsDto>> Deposite(DepositeRecord Depositerecord)
    {
        var deposit = Depositerecord with{AccountId = Depositerecord.AccountId, type = Depositerecord.type, Money = Depositerecord.Money};
        return await mediator.Send(deposit);
    } 
}
