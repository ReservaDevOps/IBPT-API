using Ibr.Core.Data;
using Ibr.IBPT.Models;

namespace Ibr.IBPT.Data.Repositories.Contracts;

public interface IIbptTaxRepository : IRepository<IbptTax>
{
    Task<IbptTax?> GetIbptTaxAsync(string uf, string code, string ex = "");
    Task<string> GetUfsForNcmApiAsync(string code);
    Task CreateIbptTaxAsync(IbptTax tax);
    void UpdateIbptTax(IbptTax tax);
}