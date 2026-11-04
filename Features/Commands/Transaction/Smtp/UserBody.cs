using System.Data.Common;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
namespace SystemBank.Features;
public class SmtpBodyCheckers
{
    public static string SmtpBodyStep(IemailSendTransaction transaction,CancellationToken token)
    {
        if (transaction.Type == TransactionType.Deposit)
        {
            return $"Ваш счёт пополнен на {transaction.MoneyTransaction}";
        }
            else
    {
        return $"Вы получили перевод от {transaction.fromUserid} на сумму {transaction.MoneyTransaction}";
    }
    }
}