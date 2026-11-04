using System.Reflection.Metadata;
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
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using MailKit.Net.Smtp;
namespace SystemBank.Features;
 public class Deposite : IRequestHandler<DepositeRecord, BaseResponse<UserTransactionsDto>>
{
    private readonly AddDb db;
    private readonly ILogger logger;
    private readonly StackExchange.Redis.IDatabase redis;
    public Deposite(AddDb _db, IConnectionMultiplexer multiplexer, ILogger<Deposite> _logger)
    {
        db = _db;
        logger = _logger;
        redis = multiplexer.GetDatabase();
    }
    public async Task<BaseResponse<UserTransactionsDto>> Handle(DepositeRecord deposite, CancellationToken token) // add a pipeline for checking
    {
        Account? account = null;
        var rediskey = $"user:v2:{deposite.AccountId}:account:type:{deposite.type}";
        var redisCache = await redis.StringGetAsync(rediskey);
        if (!redisCache.IsNull)
        {
            var cachedaccount = JsonSerializer.Deserialize<Account>(redisCache!);
            var mapping = Map.MapToDto(cachedaccount, deposite.Money);
            return BaseResponse<UserTransactionsDto>.Ok(mapping);
        }
            account = await db.accounts.FirstOrDefaultAsync(u => u.AccountId == deposite.AccountId && u.Type.TypeAccount.ToString() == deposite.type);
            if (account is null) throw new Exception("Account wasn't found");
            using var IDbContextTransaction = await db.Database.BeginTransactionAsync(token);
            try
            {
            account.Balance += deposite.Money;
            var userTransaction = new UserTransactions
        {
            Id = Guid.NewGuid().ToString(),
            Type = 0,
            ToUserId = account.AccountId,
            CreatedAt = DateTime.UtcNow,
            IsSuccess = true,
            MoneyTransaction = deposite.Money
        };
       await  db.transactions.AddAsync(userTransaction);
         await db.SaveChangesAsync(token);
                 await IDbContextTransaction.CommitAsync(token);
            }
            catch(Exception ex)
        {
            await IDbContextTransaction.RollbackAsync(token);
            logger.LogError(ex, "Deposit failed");
            throw;
        }
        var dto = Map.MapToDto(account, deposite.Money);
        return BaseResponse<UserTransactionsDto>.Ok(dto);
    }
}