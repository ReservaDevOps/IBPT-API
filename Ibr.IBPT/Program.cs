using Ibr.IBPT.Data;
using Ibr.IBPT.Data.Repositories;
using Ibr.IBPT.Data.Repositories.Contracts;
using Ibr.IBPT.Data.Seed;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 262144000;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 262144000;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IIbptTaxRepository, IbptTaxRepository>();

var app = builder.Build();

var isSqlite = app.Configuration.GetValue<string>("DatabaseProvider")
    ?.Equals("Sqlite", StringComparison.OrdinalIgnoreCase) == true;
var shouldSeedDevelopment = app.Environment.IsDevelopment();

if (isSqlite || shouldSeedDevelopment)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();

    if (isSqlite)
        db.Database.EnsureCreated();

    if (shouldSeedDevelopment)
        await DevelopmentSeed.SeedAsync(db);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
