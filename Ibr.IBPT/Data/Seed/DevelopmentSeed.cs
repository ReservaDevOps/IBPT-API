using Ibr.IBPT.Data;
using Ibr.IBPT.Models;
using Microsoft.EntityFrameworkCore;

namespace Ibr.IBPT.Data.Seed;

public static class DevelopmentSeed
{
    public static async Task SeedAsync(DataContext db)
    {
        if (await db.IBPTTaxes.AnyAsync())
            return;

        var start = DateTime.UtcNow.Date;
        var end = start.AddYears(1);

        var items = new[]
        {
            new IbptTax(
                uF: "SP",
                code: "01012100",
                ex: "",
                type: "NCM",
                description: "Bovino reprodutor",
                federalNational: 8.50m,
                importedFederal: 12.00m,
                state: 18.00m,
                municipal: 2.00m,
                startValidity: start,
                endValidity: end,
                key: "DEV-SP-01012100",
                version: "dev-1",
                source: "seed"
            ),
            new IbptTax(
                uF: "RJ",
                code: "02013000",
                ex: "",
                type: "NCM",
                description: "Carne bovina desossada",
                federalNational: 8.50m,
                importedFederal: 12.00m,
                state: 18.00m,
                municipal: 2.00m,
                startValidity: start,
                endValidity: end,
                key: "DEV-RJ-02013000",
                version: "dev-1",
                source: "seed"
            ),
            new IbptTax(
                uF: "MG",
                code: "22030000",
                ex: "",
                type: "NCM",
                description: "Cerveja de malte",
                federalNational: 8.50m,
                importedFederal: 12.00m,
                state: 18.00m,
                municipal: 2.00m,
                startValidity: start,
                endValidity: end,
                key: "DEV-MG-22030000",
                version: "dev-1",
                source: "seed"
            )
        };

        await db.IBPTTaxes.AddRangeAsync(items);
        await db.Commit();
    }
}
