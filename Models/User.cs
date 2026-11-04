using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
namespace SystemBank.Models;
public class User 
{
    public string UserID { get; set; }
    public string Login {get;set;}

    [Required] [MinLength(10,ErrorMessage = "Email can't be short"), MaxLength(64,ErrorMessage = "Email can't be long")] 
    public string Email {get;set;}
    public bool isBlock {get;set;} = false;
    public virtual List<Account> accounts {get;set;}
    public string? Role {get;set;}
}
public class UserDto
{
    public string Login {get;set;}
    public string UserID { get; set; }
    public string Email {get;set;}
    public bool isBlock {get;set;} = false;
    public string? Role {get;set;}
}
public class UserHash
{
    public User user {get;set;}
    public string Login {get;set;}
    public string Password {get;set;}
}