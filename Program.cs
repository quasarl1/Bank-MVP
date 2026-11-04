using System.Collections.Immutable;
using SystemBank;
using SystemBank.Data;
using SystemBank.Models;
using SystemBank.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using SystemBank.Features;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Connections;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "db";
var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var db_ = Environment.GetEnvironmentVariable("DB_NAME") ?? "Bank";
var user = Environment.GetEnvironmentVariable("DB_USER") ?? "myuser";
var pass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "mypassword";

var connectionString = $"Host={host};Port={port};Database={db_};Username={user};Password={pass}";


builder.Services.AddDbContext<AddDb>(options => 
    options.UseNpgsql(connectionString));
builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowAll", policy =>
   {
       policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
   }) ;
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddEndpointsApiExplorer();

var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST") ?? "localhost";
var redis = ConnectionMultiplexer.Connect($"{redisHost}:6379,abortConnect=false");
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddScoped<GetUser>();
builder.Services.AddScoped<DeleteAccount>();



builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator<User>>();
builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.name))
    .ValidateDataAnnotations()
    .ValidateOnStart();

/* var secretKey = builder.Configuration["Jwt:Key"] ?? "------------";
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        }; 
    }); */
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AddDb>();
    db.Database.Migrate();
    Console.WriteLine("Проверяем");
    if (!db.users.Any())
    {
        Console.WriteLine(" Пользователей нет");
        db.users.Add( new User
        {
            UserID = "Testik",
            Login = "da",
            Role = "Admin",
            Email = "yourEmail@gmail.com"
        });
        Console.WriteLine("First created");
        db.users.Add(new User
        {
           UserID = "Testik2",
           Login  = "da2",
           Role = "Admin",
           Email = "yourEmail@gmail.com"
        });
        Console.WriteLine("Second created");
        db.SaveChanges();
        Console.WriteLine("Пользователи созданы!");
    }
    else
    {
        Console.WriteLine("Пользователи уже есть в базе");
    }
}
app.UseCors("AllowAll");
app.UseMiddleware<IpBlocker>();
app.UseMiddleware<Checker>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();
app.Run();