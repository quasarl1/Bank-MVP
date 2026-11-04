using SystemBank.Models;
using SystemBank.Middleware;
using SystemBank;
using SystemBank.Data;
using MediatR;
using SystemBank.Controllers;
using SystemBank.Generics;
namespace SystemBank.Features;
public record CreateUserRecord(String UserId, AccountType Type) : IRequest<BaseResponse<AccountDTO>>{}