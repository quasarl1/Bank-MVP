using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
namespace SystemBank.Models;
public class Account
{
    [Key]
    public string AccountId {get;set;}
    public virtual TypeAccounted? Type { get; set; }
    public ICollection<UserTransactions> SentTransactions { get; set; }
    public decimal Balance {get;set;}
    public string UserId {get;set;}
    public DateTime DateTimeCreated {get;set;}
    public bool Success {get;set;}
    public virtual User user {get;set;}
    public bool IsBLock {get;set;} = false;
}
public class AccountDTO
{
    public string AccountId {get;set;}
    public decimal Balance {get;set;}
    public string UserId {get;set;}
    public virtual TypeAccountedDto Type { get; set; }
    public DateTime DateTimeCreated {get;set;}
    public bool Success {get;set;} 
    public bool IsBLock {get;set;} = false;
    public string Role {get;set;}
}
