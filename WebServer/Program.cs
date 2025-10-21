using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebServer.Services;
using WebServer.Model;
using WebServer.WebServer.Model.Domain.Entities;
using WebServer.WebServer.Model.Domain;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDateTimeService, SystemDateTimeService>();

// API Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for React dev server
const string AllowReact = "AllowReact";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowReact, policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure PostgreSQL connection pool
var connString = builder.Configuration.GetConnectionString("Postgres");
if (string.IsNullOrWhiteSpace(connString))
{
    throw new InvalidOperationException("Missing ConnectionStrings:Postgres in configuration");
}
builder.Services.AddSingleton(sp => new NpgsqlDataSourceBuilder(connString).Build());
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connString));
builder.Services.AddDbContext<DomainDbContext>(options =>
    options.UseNpgsql(connString));

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Enable CORS for frontend
app.UseCors(AllowReact);

// Map API controllers
app.MapControllers();

app.MapGet("/api/time", (IDateTimeService dateTimeService) =>
    Results.Ok(new { serverTimeUtc = dateTimeService.UtcNow }))
    .WithName("GetServerTime")
    .Produces(StatusCodes.Status200OK);

app.MapGet("/api/db-ping", async (NpgsqlDataSource dataSource) =>
{
    try
    {
        await using var conn = await dataSource.OpenConnectionAsync();
        await using var cmd = new NpgsqlCommand("SELECT 1", conn);
        var result = await cmd.ExecuteScalarAsync();
        return Results.Ok(new { ok = (result is int i && i == 1) });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
}).WithName("DbPing").Produces(StatusCodes.Status200OK).ProducesProblem(StatusCodes.Status500InternalServerError);

app.MapGet("/api/ef-ping", async (AppDbContext db) =>
{
    try
    {
        // Minimal EF-based ping: ask provider for a connection and SELECT 1
        var conn = db.Database.GetDbConnection();
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT 1";
        var result = await cmd.ExecuteScalarAsync();
        await conn.CloseAsync();
        return Results.Ok(new { ok = (result is int i && i == 1) });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
}).WithName("EfPing").Produces(StatusCodes.Status200OK).ProducesProblem(StatusCodes.Status500InternalServerError);

app.MapGet("/api/items", async (DomainDbContext db, int take) =>
{
    var items = await db.Titems
        .AsNoTracking()
        .Select(x => new { x.Id, x.Sku, x.Name })
        .Take(Math.Clamp(take, 1, 100))
        .ToListAsync();
    return Results.Ok(items);
}).WithName("ListItems").Produces(StatusCodes.Status200OK);

app.Run();
