using SystemBank;
using SystemBank.Models;
namespace SystemBank.Features;
public interface IemailSendTransaction
{
    public string Id {get;set;}
    public TransactionType Type { get; set; }
    public Account FromUser {get;set;}
    public string fromUserid {get;set;}
    public Account ToUser {get;set;}
    public string ToUserId {get;set;}
    public DateTime CreatedAt { get; set; }
    public decimal MoneyTransaction {get;set;}
    public bool IsSuccess { get; set; }
}