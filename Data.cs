
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using System.IO.Compression;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Data.Common;
using System.Transactions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using SystemBank.Models;
using SystemBank;
using System.ComponentModel.DataAnnotations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.Conventions;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace SystemBank.Data;
public class AddDb : DbContext
{
    public AddDb(DbContextOptions<AddDb> options) : base(options){}
    public DbSet<User> users {get;set;}
    public DbSet<UserTransactions> transactions {get;set;}
    public DbSet<Account> accounts {get;set;}
    public DbSet<TypeAccounted> types {get;set;}
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Настройка Account
    modelBuilder.Entity<Account>(entity =>
    {
        entity.HasKey(a => a.AccountId);
        entity.Property(a => a.AccountId).HasDefaultValueSql("gen_random_uuid()");
        entity.HasIndex(a => a.AccountId).IsUnique();

        entity.HasOne(a => a.user)
              .WithMany(u => u.accounts)
              .HasForeignKey(a => a.UserId);
        entity.HasOne(a => a.Type)
              .WithOne(t => t.account)
              .HasForeignKey<TypeAccounted>(t => t.AccountId);
    });
    // Настройка транзакций(чек транзакций)
    modelBuilder.Entity<UserTransactions>(entity =>
    {
        entity.HasKey(t => t.Id);
        entity.Property(t => t.Id).HasDefaultValueSql("gen_random_uuid()");

        entity.HasOne(t => t.FromUser)
              .WithMany(a => a.SentTransactions)
              .HasForeignKey(t => t.fromUserid)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(t => t.ToUser)
              .WithMany()
              .HasForeignKey(t => t.ToUserId)
              .OnDelete(DeleteBehavior.Restrict);
    });
}
        
    } 

