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
namespace SystemBank.Features;
public class CreateUserHandler: IRequestHandler<CreateUserRecord, BaseResponse<AccountDTO>>
{
     private readonly StackExchange.Redis.IDatabase redis;
    private readonly AddDb db;
    public CreateUserHandler(IConnectionMultiplexer multiplexer,AddDb _db)
    {
        db =_db;
        redis = multiplexer.GetDatabase();
    }
    public async Task<BaseResponse<AccountDTO>> Handle(CreateUserRecord request, CancellationToken token)
    {
    
        var user = await db.users.Include(u => u.accounts).ThenInclude(u => u.Type).AsSplitQuery().FirstOrDefaultAsync(u => u.UserID == request.UserId); // ADD SELECT()
        if (user is null) throw new Exception("didn't found in database");
        if (user.accounts.Count >= 4) throw new Exception("You have > 4 accounts");
        if (!Enum.IsDefined(typeof(AccountType), request.Type)) throw new Exception("not corrently account type");
        var account = new Account
        {
            UserId = user.UserID,
            Balance = 0,
            Success = true,
            Type = new TypeAccounted
            {
              TypeAccount = request.Type  

            },
            DateTimeCreated = DateTime.UtcNow,
            IsBLock = false
        };
        await db.accounts.AddAsync(account);
        await db.SaveChangesAsync();
        string rediskey = $"user:v2:{user.UserID}:accounts:type:{account.Type.TypeAccount}";
        await redis.SetAddAsync(rediskey, account.AccountId);
        var acc = new AccountDTO
        {
            AccountId = account.AccountId,
            UserId = account.UserId,
            Balance = account.Balance,
            Success = account.Success,
            Type = new TypeAccountedDto
            {
                AccountId = account.AccountId,
                TypeAccount = account.Type.TypeAccount
            },
            DateTimeCreated = account.DateTimeCreated,
            IsBLock = account.IsBLock
        };
        return BaseResponse<AccountDTO>.Ok(acc);
    }
}
