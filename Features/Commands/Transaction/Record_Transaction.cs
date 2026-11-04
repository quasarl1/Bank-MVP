using System;
using System.IO;
using SystemBank.Models;
using SystemBank.Middleware;
using SystemBank;
using SystemBank.Data;
using MediatR;
using SystemBank.Generics;
using SystemBank.Controllers;
using Microsoft.EntityFrameworkCore;
using SystemBank.Features;
namespace SystemBank.Features;
public record TransactionRecord(string FromUserId, AccountType Fromtypeaccount, string ToUserId, decimal Money) : IRequest<BaseResponse<UserTransactionsDto>>{}