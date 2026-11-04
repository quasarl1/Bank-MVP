using System.Reflection.Metadata;
using System;
using System.IO;
using SystemBank.Models;
using SystemBank.Middleware;
using SystemBank;
using SystemBank.Data;
using MediatR;
using SystemBank.Generics;
using SystemBank.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace SystemBank.Features;
public class Map {
public static UserTransactionsDto MapToDto(Account account, decimal money) => new()
{
        Id = Guid.NewGuid().ToString(),
            Type = 0,
            ToUserId = account.AccountId,
            CreatedAt = DateTime.UtcNow,
            IsSuccess = true,
            MoneyTransaction = money
};
}
