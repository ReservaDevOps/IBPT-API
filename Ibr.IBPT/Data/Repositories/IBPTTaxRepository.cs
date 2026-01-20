using Ibr.Core.Data;
using Ibr.IBPT.Data.Repositories.Contracts;
using Ibr.IBPT.Models;
using Microsoft.EntityFrameworkCore;

namespace Ibr.IBPT.Data.Repositories;

public class IbptTaxRepository : IIbptTaxRepository
{
    private readonly DataContext _dataContext;

    public IUnitOfWork UnitOfWork => _dataContext;

    public IbptTaxRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IbptTax?> GetIbptTaxAsync(string uf, string code, string ex = "")
    {
        return await _dataContext.IBPTTaxes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.UF.Trim().ToUpper() == uf.Trim().ToUpper() &&
                                      x.Code.Trim() == code.Trim() &&
                                      x.Ex.Trim() == ex.Trim()
            );
    }

    public async Task<string> GetUfsForNcmApiAsync(string code)
    {
        var ufs = await _dataContext.IBPTTaxes
            .AsNoTracking()
            .Where(x => x.Code == code)
            .Select(x => x.UF)
            .ToListAsync();

        return string.Join(", ", ufs);
    }

    public async Task CreateIbptTaxAsync(IbptTax tax)
    {
        await _dataContext.IBPTTaxes.AddAsync(tax);
    }

    public void UpdateIbptTax(IbptTax tax)
    {
        _dataContext.Entry(tax).State = EntityState.Modified;
    }

    public void Dispose()
    {
        _dataContext.Dispose();
    }
}