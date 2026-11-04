using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
namespace SystemBank.Generics;
public class BaseResponse<T> 
{
    public bool Success {get;set;}
    public DateTime Time {get;set;} = DateTime.UtcNow;
    public int CountErrors {get;set;}
    public List<string> ListErrors{get;set;} = new();
    public T Data {get;set;}
    public string Message {get;set;}
    public static BaseResponse<T> Ok(T data, string message = "Success") => new() {Data = data, Message = message, Success = true};
    public static BaseResponse<T> Fail(string message = "failed",
    List<string> ListErrors = null) 
       => new() 
    {
    Message = message,
    Success = false,
    Data = default,
    ListErrors = ListErrors ?? new List<string>()
    };
    public static BaseResponse<IEnumerable<T>> Delete(string message = "success deleted") => new() 
    {Success = true, Message = message, Data = Enumerable.Empty<T>()}; //FIX
}