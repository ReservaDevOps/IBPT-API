using Ibr.IBPT.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Ibr.IBPT.Data.Seed;

public class IbptSeedRunner
{
    private readonly IIbptImportService _importService;
    private readonly IConfiguration _config;
    private readonly IHostEnvironment _env;

    public IbptSeedRunner(
        IIbptImportService importService,
        IConfiguration config,
        IHostEnvironment env)
    {
        _importService = importService;
        _config = config;
        _env = env;
    }

    public async Task RunAsync()
    {
        if (!_env.IsDevelopment()) return;
        if (!_config.GetValue<bool>("SeedData:Enabled")) return;

        var basePath = Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..",
            "Data", "Seed", "2025"
        );

        foreach (var file in Directory.GetFiles(basePath, "*.csv"))
        {
            var uf = Path.GetFileNameWithoutExtension(file).ToUpper();
            if (uf.Length != 2 || !FileService.UfIsValid(uf))
                continue;

            using var stream = File.OpenRead(file);
            await _importService.ImportCsvAsync(uf, stream);
        }
    }
}
