
using SystemBank.Models;
using SystemBank.Features;
namespace SystemBank;
    public enum TransactionType
    {
        Deposit = 0,
        Transfer = 1
    }
public class UserTransactions : IemailSendTransaction
{
    public string Id {get;set;}
    public TransactionType Type { get; set; }
    public Account? FromUser {get;set;}
    public string? fromUserid {get;set;}
    public Account? ToUser {get;set;}
    public string ToUserId {get;set;}
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal MoneyTransaction {get;set;}
    public bool IsSuccess { get; set; }
}
public class UserTransactionsDto
{
    public string Id {get;set;}
    public TransactionType Type { get; set; }
    public string? YooKassaPaymentId { get; set; }        
    public string? FromUserid {get;set;}
    public string ToUserId {get;set;}
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal MoneyTransaction {get;set;}
    public bool IsSuccess { get; set; }
    public string? PaymentUrl { get; set; }

}