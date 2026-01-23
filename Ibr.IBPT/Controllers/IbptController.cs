using Microsoft.AspNetCore.Mvc;
using Ibr.IBPT.Data.Repositories.Contracts;
using Ibr.IBPT.Services;

namespace Ibr.IBPT.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class IbptController : ControllerBase
{
    private readonly IIbptTaxRepository _ibptTaxRepository;
    private readonly IIbptImportService _importService;

    public IbptController(
        IIbptTaxRepository ibptTaxRepository,
        IIbptImportService importService)
    {
        _ibptTaxRepository = ibptTaxRepository;
        _importService = importService;
    }

    // =========================
    // GET /api/v1/ibpt
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetIbptTax(
        [FromQuery] string? uf,
        [FromQuery] string? code)
    {
        if (string.IsNullOrWhiteSpace(uf) || string.IsNullOrWhiteSpace(code))
            return BadRequest(new { Message = "Informe os parâmetros corretamente." });

        var ibpt = await _ibptTaxRepository
            .GetIbptTaxAsync(uf, code.PadRight(8, '0'));

        return ibpt is not null ? Ok(ibpt) : NotFound();
    }

    // =========================
    // GET /api/v1/ibpt/GetUfsForNcmApi
    // =========================
    [HttpGet("GetUfsForNcmApi")]
    public async Task<IActionResult> GetUfsForNcmApi(
        [FromQuery] string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BadRequest(new { Message = "Informe um código." });

        if (code.Length < 8)
            return BadRequest(new { Message = "Informe o código corretamente." });

        var ncmUfs = await _ibptTaxRepository
            .GetUfsForNcmApiAsync(code.PadRight(8, '0'));

        return !string.IsNullOrEmpty(ncmUfs) ? Ok(ncmUfs) : NotFound();
    }

    // =========================
    // POST /api/v1/ibpt
    // CONTEXTO PRINCIPAL DO DESAFIO
    // =========================
    [HttpPost]
    public async Task<IActionResult> UploadIbptTaxFiles(
        [FromForm] IEnumerable<IFormFile> files)
    {
        try
        {
            if (files == null || !files.Any())
                return BadRequest(new { Message = "Informe ao menos um arquivo válido." });

            foreach (var file in files)
            {
                if (file.Length == 0)
                    continue;

                if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    return BadRequest(new { Message = "Envie apenas arquivos com a extensão .csv!" });

                // Regra do projeto: nome do arquivo = UF.csv
                var uf = Path.GetFileNameWithoutExtension(file.FileName)
                    .Trim()
                    .ToUpperInvariant();

                if (uf.Length != 2)
                    return BadRequest(new { Message = "Informe a UF no nome do arquivo." });

                if (!FileService.UfIsValid(uf))
                    return BadRequest(new { Message = "Informe uma UF válida." });

                using var stream = file.OpenReadStream();
                await _importService.ImportCsvAsync(uf, stream);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = "Um erro inesperado aconteceu.",
                Details = ex.Message
            });
        }
    }
}
