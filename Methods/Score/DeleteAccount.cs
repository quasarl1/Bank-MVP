using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SystemBank.Data;
using SystemBank.Generics;
using SystemBank.Models;

public class DeleteAccount
{
    private readonly AddDb db;
    public DeleteAccount(AddDb _db) => db = _db;
    public async Task<BaseResponse<IEnumerable<AccountDTO>>> Delete(string id)
    {
        var user = await db.users.Include(u => u.accounts).FirstOrDefaultAsync(u => u.UserID == id);
        if (user is null) throw new Exception("user id isn't found");
        var acc = user.accounts.Select(u=> u.AccountId).ToList();
        var AccountToDelete = db.accounts.Where(u => acc.Contains(u.AccountId));
        db.accounts.RemoveRange(AccountToDelete);
        if (!AccountToDelete.Any()) throw new Exception("isn't deleted");
        await db.SaveChangesAsync();
        return BaseResponse<IEnumerable<AccountDTO>>.Ok(Enumerable.Empty<AccountDTO>(), "accounts has been deleted");
    }
}