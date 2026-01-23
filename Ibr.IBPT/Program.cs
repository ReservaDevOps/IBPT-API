using Ibr.IBPT.Data;
using Ibr.IBPT.Data.Repositories;
using Ibr.IBPT.Data.Repositories.Contracts;
using Ibr.IBPT.Data.Seed;
using Ibr.IBPT.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Database
// =========================
builder.Services.AddDbContext<DataContext>(options =>
{
    var provider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "Postgres";

    if (provider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
    }
    else
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
    }
});

// =========================
// Upload limits (CSV)
// =========================
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 262_144_000;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 262_144_000;
});

// =========================
// MVC + Swagger
// =========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =========================
// Dependency Injection
// =========================
builder.Services.AddTransient<IIbptTaxRepository, IbptTaxRepository>();

// ?? NOVO: serviço de importação reutilizável
builder.Services.AddScoped<IIbptImportService, IbptImportService>();

// ?? NOVO: seed runner
builder.Services.AddScoped<IbptSeedRunner>();

var app = builder.Build();

// =========================
// Database init + Seed
// =========================
var isSqlite = app.Configuration.GetValue<string>("DatabaseProvider")
    ?.Equals("Sqlite", StringComparison.OrdinalIgnoreCase) == true;

if (isSqlite)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.EnsureCreated();
}

// ?? Seed automático SOMENTE em Development + flag
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seedRunner = scope.ServiceProvider.GetRequiredService<IbptSeedRunner>();
    await seedRunner.RunAsync();
}

// =========================
// HTTP pipeline
// =========================
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
