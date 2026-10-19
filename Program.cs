using dotnetapi.Context;
using dotnetapi.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using DotNetEnv;



var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliticaCORS", policy =>
    {
        policy.WithOrigins("http://localhost:4200",
            "http://localhost",
            "http://127.0.0.1:4200",
            "http://192.168.123.43:4200",
            "capacitor://localhost",
            "ionic://localhost"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ProductService>();

builder.Services.AddScoped<CategoryService>();

builder.Services.AddScoped<InventoryService>();


builder.Services.AddScoped<SaleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

}

app.UseHttpsRedirection();

app.UseCors("PoliticaCORS");

app.UseAuthorization();

app.MapControllers();

app.Run();
