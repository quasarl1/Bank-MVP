using System.Collections.Immutable;
using System.Data.Common;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Identity.Client;
using SystemBank.Data;
using SystemBank.Generics;
using SystemBank.Models;
namespace SystemBank.Features;
public class SendEmail {
    private static IConfiguration _config;
    public SendEmail(IConfiguration config) => _config = config;
public static async Task<T> SendEmailTo<T>(T data,AddDb db, CancellationToken token) where T : IemailSendTransaction
    {
        var accountid = data.Type switch
        {
            TransactionType.Deposit => data.ToUserId,
            TransactionType.Transfer => data.fromUserid,
            _ => throw new Exception("Unknown transaction type")
        };
var user = await db.users
    .Include(u => u.accounts)
    .FirstOrDefaultAsync(u => u.accounts.Any(a => a.AccountId == accountid), token);
if (user is null) throw new Exception("User didn't find");
        var userSmtp = _config["Smtp:Username"] ?? throw new InvalidOperationException("Smtp:Username not set");
        var passSmtp = _config["Smtp:Password"] ?? throw new InvalidOperationException("Smtp:Password not set");
                var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(userSmtp, passSmtp),
        };
        var body = SmtpBodyCheckers.SmtpBodyStep(data,token);
        var mail = new MailMessage()
        {
            From = new MailAddress("maksimka1243124@gmail.com"),
            Subject = "",
            Body = body,
            IsBodyHtml = true,
        };
        mail.To.Add(user.Email);
        await smtp.SendMailAsync(mail,token);
        return data;
    }
}