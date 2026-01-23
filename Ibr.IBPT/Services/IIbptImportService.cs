using System.Globalization;
using Ibr.IBPT.Data.Repositories.Contracts;
using Ibr.IBPT.Models;

namespace Ibr.IBPT.Services;

public interface IIbptImportService
{
    Task ImportCsvAsync(string uf, Stream csvStream);
}

public class IbptImportService : IIbptImportService
{
    private readonly IIbptTaxRepository _repository;
    private static readonly CultureInfo DecimalCulture = new("en-US");

    public IbptImportService(IIbptTaxRepository repository)
    {
        _repository = repository;
    }

    public async Task ImportCsvAsync(string uf, Stream csvStream)
    {
        var lines = FileService.GetLinesToInsert(csvStream);
        List<string> headers = new();

        for (int i = 0; i < lines.Length; i++)
        {
            try
            {
                var columns = FileService.CleanColumns(lines[i].Split(";"));

                if (i == 0)
                {
                    headers = columns.ToList();
                    continue;
                }

                if (columns.Length != headers.Count)
                    continue;

                string code = columns[headers.IndexOf("codigo")].PadRight(8, '0');
                string ex = columns[headers.IndexOf("ex")];
                string type = columns[headers.IndexOf("tipo")];
                string description = columns[headers.IndexOf("descricao")];

                decimal federalNational =
                    string.IsNullOrWhiteSpace(columns[headers.IndexOf("nacionalfederal")]) ? 0 :
                    Convert.ToDecimal(columns[headers.IndexOf("nacionalfederal")], DecimalCulture);

                decimal importedFederal =
                    string.IsNullOrWhiteSpace(columns[headers.IndexOf("importadosfederal")]) ? 0 :
                    Convert.ToDecimal(columns[headers.IndexOf("importadosfederal")], DecimalCulture);

                decimal state =
                    string.IsNullOrWhiteSpace(columns[headers.IndexOf("estadual")]) ? 0 :
                    Convert.ToDecimal(columns[headers.IndexOf("estadual")], DecimalCulture);

                decimal municipal =
                    string.IsNullOrWhiteSpace(columns[headers.IndexOf("municipal")]) ? 0 :
                    Convert.ToDecimal(columns[headers.IndexOf("municipal")], DecimalCulture);

                DateTime startValidity =
                    string.IsNullOrWhiteSpace(columns[headers.IndexOf("vigenciainicio")]) ? DateTime.UtcNow.Date :
                    FileService.GetDate(columns[headers.IndexOf("vigenciainicio")]).Date;

                DateTime endValidity =
                    string.IsNullOrWhiteSpace(columns[headers.IndexOf("vigenciafim")]) ? DateTime.UtcNow.Date :
                    FileService.GetDate(columns[headers.IndexOf("vigenciafim")]).Date;

                string key = columns[headers.IndexOf("chave")];
                string version = columns[headers.IndexOf("versao")];
                string source = columns[headers.IndexOf("fonte")];

                var existing = await _repository.GetIbptTaxAsync(uf, code, ex);

                if (existing == null)
                {
                    await _repository.CreateIbptTaxAsync(new IbptTax(
                        uf, code, ex, type, description,
                        federalNational, importedFederal, state, municipal,
                        startValidity, endValidity, key, version, source
                    ));
                }
                else
                {
                    existing.SetIbptTax(
                        type, description,
                        federalNational, importedFederal, state, municipal,
                        startValidity, endValidity, key, version, source
                    );

                    _repository.UpdateIbptTax(existing);
                }

                if (i % 1000 == 0)
                    await _repository.UnitOfWork.Commit();
            }
            catch { }
        }

        await _repository.UnitOfWork.Commit();
    }
}
