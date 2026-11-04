using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemBank.Data;
using SystemBank.Generics;
using SystemBank.Models;

namespace SystemBank.Features;
public class GetUser
{
    private readonly AddDb db;
    public GetUser(AddDb _db) => db = _db;
    public async Task<BaseResponse<List<AccountDTO>>>  Get(string id)
    {
        var user = await db.users.AsNoTracking().Include(u=> u.accounts).ThenInclude(u => u.Type).FirstOrDefaultAsync(u => u.UserID == id);
            if (user is null) throw new Exception("user isn't found");
        var dto = user.accounts.Select(a => new AccountDTO
        {
        AccountId = a.AccountId,
        UserId = a.UserId,
        Balance = a.Balance,
        IsBLock = a.IsBLock,
        Type = new TypeAccountedDto
        {
            AccountId =  a.AccountId,
            TypeAccount = a.Type.TypeAccount,
        }
        }).ToList();
        return BaseResponse<List<AccountDTO>>.Ok(dto);
    }
}