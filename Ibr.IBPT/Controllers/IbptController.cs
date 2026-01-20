using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Ibr.IBPT.Data.Repositories.Contracts;
using Ibr.IBPT.Models;
using Ibr.IBPT.Services;

namespace Ibr.IBPT.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class IbptController : ControllerBase
{
    private readonly IIbptTaxRepository _ibptTaxRepository;

    public IbptController(IIbptTaxRepository ibptTaxRepository)
    {
        _ibptTaxRepository = ibptTaxRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetIbptTax(
        [FromQuery] string? uf,
        [FromQuery] string? code
    )
    {
        if (string.IsNullOrWhiteSpace(uf) || string.IsNullOrWhiteSpace(code))
            return BadRequest(new { Message = "Informe os parâmetros corretamente." });

        var ibpt = await _ibptTaxRepository.GetIbptTaxAsync(uf, code.PadRight(8, '0'));
        return ibpt is not null ? Ok(ibpt) : NotFound();
    }

    [HttpGet("GetUfsForNcmApi")]
    public async Task<IActionResult> GetUfsForNcmApi([FromQuery] string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BadRequest(new { Message = "Informe um código." });

        if (code.Length < 8)
            return BadRequest(new { Message = "Informe o código corretamente." });

        var ncmUfs = await _ibptTaxRepository.GetUfsForNcmApiAsync(code.PadRight(8, '0'));

        return !string.IsNullOrEmpty(ncmUfs) ? Ok(ncmUfs) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> UploadIbptTaxFiles([FromForm] IEnumerable<IFormFile> files)
    {
        CultureInfo cultureInfo = new CultureInfo("en-US");

        try
        {
            if (files == null || !files.Any())
                return BadRequest(new { Message = "Informe ao menos um arquivo válido." });

            foreach (var file in files)
            {
                if (file.Length == 0)
                    continue;

                if (!file.FileName.EndsWith(".csv"))
                    return BadRequest(new { Message = "Envie apenas arquivos com a extensão .csv!" });

                var uf = file.FileName.Trim().Split(".csv")[0];
                if (uf.Length != 2)
                    return BadRequest(new { Message = "Informe a UF no nome do arquivo." });

                uf = uf.Trim().ToUpper();
                if (!FileService.UfIsValid(uf))
                    return BadRequest(new { Message = "Informe uma UF válida." });

                var lines = FileService.GetLinesToInsert(file);
                List<string> headers = new List<string>();

                for (var i = 0; i < lines.Length; i++)
                {
                    try
                    {
                        var columns = lines[i].Split(";");
                        columns = FileService.CleanColumns(columns);

                        if (i == 0)
                            headers = columns.ToList();
                        else if (columns.Length == headers.Count)
                        {
                            string code = columns[headers.IndexOf("codigo")].PadRight(8, '0');
                            string ex = columns[headers.IndexOf("ex")];
                            string type = columns[headers.IndexOf("tipo")];
                            string description = columns[headers.IndexOf("descricao")];
                            decimal federalNational = string.IsNullOrWhiteSpace(columns[headers.IndexOf("nacionalfederal")]) ? 0 :
                                Convert.ToDecimal(columns[headers.IndexOf("nacionalfederal")], cultureInfo);
                            decimal importedFederal = string.IsNullOrWhiteSpace(columns[headers.IndexOf("importadosfederal")]) ? 0 :
                                Convert.ToDecimal(columns[headers.IndexOf("importadosfederal")], cultureInfo);
                            decimal state = string.IsNullOrWhiteSpace(columns[headers.IndexOf("estadual")]) ? 0 :
                                Convert.ToDecimal(columns[headers.IndexOf("estadual")], cultureInfo);
                            decimal municipal = string.IsNullOrWhiteSpace(columns[headers.IndexOf("municipal")]) ? 0 :
                                Convert.ToDecimal(columns[headers.IndexOf("municipal")], cultureInfo);
                            DateTime startValidity = string.IsNullOrWhiteSpace(columns[headers.IndexOf("vigenciainicio")]) ? DateTime.UtcNow.Date :
                                FileService.GetDate(columns[headers.IndexOf("vigenciainicio")]).Date;
                            DateTime endValidity = string.IsNullOrWhiteSpace(columns[headers.IndexOf("vigenciafim")]) ? DateTime.UtcNow.Date :
                                FileService.GetDate(columns[headers.IndexOf("vigenciafim")]).Date;
                            string key = columns[headers.IndexOf("chave")];
                            string version = columns[headers.IndexOf("versao")];
                            string source = columns[headers.IndexOf("fonte")];

                            var ibptTaxSaved = await _ibptTaxRepository.GetIbptTaxAsync(uf, code, ex);
                            if (ibptTaxSaved == null)
                            {
                                var tax = new IbptTax(
                                    uF: uf,
                                    code: code,
                                    ex: ex,
                                    type: type,
                                    description: description,
                                    federalNational: federalNational,
                                    importedFederal: importedFederal,
                                    state: state,
                                    municipal: municipal,
                                    startValidity: startValidity,
                                    endValidity: endValidity,
                                    key: key,
                                    version: version,
                                    source: source
                                );

                                await _ibptTaxRepository.CreateIbptTaxAsync(tax);
                            }
                            else
                            {
                                ibptTaxSaved.SetIbptTax(
                                    type: type,
                                    description: description,
                                    federalNational: federalNational,
                                    importedFederal: importedFederal,
                                    state: state,
                                    municipal: municipal,
                                    startValidity: startValidity,
                                    endValidity: endValidity,
                                    key: key,
                                    version: version,
                                    source: source
                                );

                                _ibptTaxRepository.UpdateIbptTax(ibptTaxSaved);
                            }

                            if (i % 1000 == 0)
                            {
                                await _ibptTaxRepository.UnitOfWork.Commit();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error with details of the current line or file.
                        // Example: _logger.LogError(ex, "Erro ao processar a linha {LineIndex} do arquivo {FileName}", i, file.FileName);
                        // Continue processing the next line or file
                    }
                }
            }

            await _ibptTaxRepository.UnitOfWork.Commit();
        }
        catch (Exception ex)
        {
            _ibptTaxRepository.Dispose();
            return BadRequest(new { Message = "Um erro inesperado aconteceu.", Details = ex.Message });
        }

        return Ok();
    }
}