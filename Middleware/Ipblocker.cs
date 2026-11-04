using System.Net;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Options;
using SystemBank.Data;

public class IpBlocker
{
    private readonly RequestDelegate next;
     private readonly List<string> _bannedIps;
    public IpBlocker(RequestDelegate _next,IConfiguration configuration)
    {
        next = _next;
         _bannedIps = configuration.GetSection("bannedIp").Get<List<string>>() ?? new List<string>();
    }
    public async Task InvokeAsync(HttpContext http) 
    {
        var ip = http.Connection.RemoteIpAddress?.ToString();
        await Block(ip, http);
        
    }
    public async Task Block(string ip, HttpContext http)
    { 
        if (string.IsNullOrEmpty(ip))
        {
            await next(http);
            return;
        }
        if (_bannedIps.Contains(ip)) 
        {
        await http.Response.WriteAsync("Access Denied: Your ip is banned");
        return ;
        }
        await next(http);
    }
}