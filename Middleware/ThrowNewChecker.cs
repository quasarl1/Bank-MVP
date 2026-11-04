using System.ComponentModel.DataAnnotations;
using System.Net;
using SystemBank.Generics;
namespace SystemBank.Middleware;
public class Checker
{
    private readonly RequestDelegate _next;
    private readonly ILogger<Checker> _logger;
    public Checker(RequestDelegate next, ILogger<Checker> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task Invoke(HttpContext http)
    {
        try
        {
            await _next(http);
        }
        catch(ValidationException ex)
        {
            _logger.LogWarning(ex,"Validation Erorr: {Message}", ex.Message);
            await MethodRequest(http,ex.Message, HttpStatusCode.BadRequest);
        }
        catch (DllNotFoundException ex)
        {
            _logger.LogWarning(ex,"Not found: {Message}", ex.Message);
            await MethodRequest(http,ex.Message, HttpStatusCode.NotFound);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex, "Unhandled error: {Message}", ex.Message);
            await MethodRequest(http, ex.Message, HttpStatusCode.InternalServerError);
        }
    }
    private static Task MethodRequest(HttpContext http,string message,HttpStatusCode status)
    {
        http.Response.ContentType = "application/json";
        http.Response.StatusCode = (int)status;
        var response = BaseResponse<object>.Fail(message);
        return http.Response.WriteAsJsonAsync(response);
    }
}