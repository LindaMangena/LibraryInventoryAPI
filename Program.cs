using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using LibraryInventoryAPI.Data;
using LibraryInventoryAPI.Models.DTOs;
using LibraryInventoryAPI.Services;
using LibraryInventoryAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseInMemoryDatabase("LibraryInventory"));


var secretKey = "4B0A1E2D3C4F5A6B7C8D9E0F1A2B3C4D5E6F7G8H9I0J1K2L3M4N5O6P7Q8R9S0T";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Register services
builder.Services.AddSingleton<ITokenService>(new TokenService(secretKey));

// Register validators
builder.Services.AddTransient<IValidator<BookCreateDto>, BookCreateValidator>();
builder.Services.AddTransient<IValidator<BookUpdateDto>, BookUpdateValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();  
app.UseMiddleware<RequestResponseLoggingMiddleware>(); 

app.UseAuthentication();  
app.UseAuthorization();    

app.MapControllers();

app.Run();
