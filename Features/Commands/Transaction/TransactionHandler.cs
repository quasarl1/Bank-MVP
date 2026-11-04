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
using SystemBank.Features;
using AutoMapper;
namespace SystemBank.Features;
public class Transaction : IRequestHandler<TransactionRecord, BaseResponse<UserTransactionsDto>> 
{
    private readonly AddDb db;
    public Transaction(AddDb _db)
    {
        db = _db;
    }
    public async Task<BaseResponse<UserTransactionsDto>> Handle(TransactionRecord record, CancellationToken token)
    {
        var fromUserid = await db.users.Include(u => u.accounts).ThenInclude(u=> u.Type).FirstOrDefaultAsync(u => u.UserID == record.FromUserId, token);
        if (fromUserid is null) throw new Exception("Your id isn't found in the database");;
        if (record.Money >= 500000) throw new Exception("Transfer will not be able to over than 500000 and <= 0"); // will add record.money <= 0 ||
        var toUserid = await db.users.Include(u => u.accounts).ThenInclude(u=> u.Type).FirstOrDefaultAsync(u => u.UserID == record.ToUserId, token);
        if (toUserid is null) throw new Exception("Is not valid id");

        if (!Enum.IsDefined(typeof(AccountType), record.Fromtypeaccount)) throw new Exception("not corrently account type");

        var FromUserAccount = fromUserid.accounts.FirstOrDefault(u => u.Type.TypeAccount == record.Fromtypeaccount);
        if (FromUserAccount is null) throw new Exception("You don't have any account with this type");

        var ToUserAccount = toUserid.accounts.FirstOrDefault(u => u.Type.TypeAccount == AccountType.Checking);
        if (ToUserAccount is null) throw new Exception("User doesn't have any account with this type");
        FromUserAccount.Balance -= record.Money;
        ToUserAccount.Balance += record.Money;
        var transfer = new UserTransactions
        {
            fromUserid = FromUserAccount.AccountId,
            FromUser = FromUserAccount,
            ToUserId = ToUserAccount.AccountId,
            ToUser = ToUserAccount,
            CreatedAt = DateTime.UtcNow,
            MoneyTransaction = record.Money,
            IsSuccess = true,
        };
        await db.transactions.AddAsync(transfer, token);
        using var transactionscope = await db.Database.BeginTransactionAsync(token);
        try {
        await db.SaveChangesAsync(token);
        await transactionscope.CommitAsync(token);
        }
        catch
        {
            await transactionscope.RollbackAsync(token);
            throw;
        }
        var dto = new UserTransactionsDto 
        {
            Id = transfer.Id,
            FromUserid= transfer.fromUserid,
            ToUserId = transfer.ToUserId,
            
            CreatedAt = transfer.CreatedAt,
            MoneyTransaction = transfer.MoneyTransaction,
            IsSuccess = transfer.IsSuccess
        };
        await SendEmail.SendEmailTo(transfer,db,token);
        return BaseResponse<UserTransactionsDto>.Ok(dto);
    }
}