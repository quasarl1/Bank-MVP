using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace SystemBank.Models;

public enum AccountType
{
Checking,
Savings,
Credit,
Investment,
}
[Index(nameof(AccountId), nameof(TypeAccount), IsUnique = true)]
public class TypeAccounted {
    [Key]
    public string AccountId {get;set;}
    public AccountType TypeAccount {get;set;}
    public Account account {get;set;}
    public int Bonuses {get;set;} = 0;
}
public class TypeAccountedDto
{
    public string AccountId {get;set;}
    public AccountType TypeAccount {get;set;}
    public int Bonuses {get;set;} = 0;
}