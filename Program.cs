using dotnetapi.Context;
using dotnetapi.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

LoadDotEnv();

var builder = WebApplication.CreateBuilder(args);

static void LoadDotEnv()
{
    var roots = new[] { new DirectoryInfo(Directory.GetCurrentDirectory()), new DirectoryInfo(AppContext.BaseDirectory) };
    foreach (var root in roots)
    {
        var dir = root;
        while (dir is not null)
        {
            var path = Path.Combine(dir.FullName, ".env");
            if (!File.Exists(path))
            {
                dir = dir.Parent;
                continue;
            }

            foreach (var line in File.ReadLines(path))
            {
                var entry = line.Trim();
                if (entry.Length == 0 || entry.StartsWith('#')) continue;
                if (entry.StartsWith("export ")) entry = entry[7..].Trim();

                var separator = entry.IndexOf('=');
                if (separator <= 0) continue;

                var key = entry[..separator].Trim();
                var value = entry[(separator + 1)..].Trim();
                if (value.Length >= 2 && (value[0] == '"' || value[0] == '\'') && value[^1] == value[0])
                    value = value[1..^1];

                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
                    Environment.SetEnvironmentVariable(key, value);
            }
            return;
        }
    }
}

static string BuildConnectionString(IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    foreach (var key in new[] { "DB_USER", "DB_PASSWORD", "DB_NAME" })
        connectionString = connectionString.Replace($"{{{key}}}", Environment.GetEnvironmentVariable(key) ?? string.Empty);

    if (connectionString.Contains('{'))
        throw new InvalidOperationException("Faltan valores en .env: define DB_USER, DB_PASSWORD y DB_NAME.");

    return connectionString;
}


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
    options.UseSqlServer(BuildConnectionString(builder.Configuration)));

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
