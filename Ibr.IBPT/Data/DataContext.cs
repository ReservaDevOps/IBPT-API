using Ibr.Core.Data;
using Ibr.IBPT.Models;
using Microsoft.EntityFrameworkCore;

namespace Ibr.IBPT.Data;

public class DataContext : DbContext, IUnitOfWork
{
    public DbSet<IbptTax> IBPTTaxes => Set<IbptTax>();

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IbptTax>()
            .HasKey(ibpt => new { ibpt.UF, ibpt.Code, ibpt.Ex });

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit()
    {
        return await base.SaveChangesAsync() > 0;
    }
}