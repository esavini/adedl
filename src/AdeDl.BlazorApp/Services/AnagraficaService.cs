using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;
using Microsoft.Extensions.Logging;

namespace AdeDl.BlazorApp.Services;

public class AnagraficaService : IAnagraficaService
{
    private readonly ICassettoFiscaleService _cassettoFiscaleService;

    private readonly ICredentialService _credentialService;

    private readonly IFileDownloaderService _fileDownloaderService;

    private readonly ILogger<AnagraficaService> _logger;

    public AnagraficaService(ICassettoFiscaleService cassettoFiscaleService, ICredentialService credentialService,
        IFileDownloaderService fileDownloaderService, ILogger<AnagraficaService> logger)
    {
        _cassettoFiscaleService = cassettoFiscaleService;
        _credentialService = credentialService;
        _fileDownloaderService = fileDownloaderService;
        _logger = logger;
    }

    public async Task DownloadAnagraficaAsync(Customer customer, Anagrafica operation,
        CancellationToken cancellationToken)
    {
        var currentCredential = await _credentialService.GetCurrentCredentialAsync();
        var browserService = await _cassettoFiscaleService.OpenRawCassettoAsync(customer, true);

        var filesToDownload = new[]
        {
            ("Ana", "dati anagrafici.pdf", "Dati Anagrafici"),
            ("dRapp", "rappresentanze.pdf", "Rappresentanze"),
            ("AltreAtt", "altre attivita.pdf", "Altre Attivita'"),
            ("AltreSed", "altri luoghi di esercizio.pdf", "Altri Luoghi di Esercizio"),
            ("Dep", "depositari.pdf", "Depositari"),
        };

        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var subFolderPath = Path.Combine(path, "AdeDl", currentCredential.Name, customer.Name,
            $"{DateTime.Now:yyyy.MM.dd} - Verifica anagrafica - Cassetto fiscale");

        var prefix =
            $"https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?stampa=S&CF={customer.FiscalCode}&Ric=";

        await Task.Delay(2000, cancellationToken);

        Directory.CreateDirectory(subFolderPath);

        var toDownload =
            await browserService.ActAsync<string[]>(
                "Array.from(document.querySelectorAll('li.indent a')).map(a => a.innerText.trim())");

        foreach (var (url, file, btnName) in filesToDownload.Where(t => toDownload.Contains(t.Item3)))
        {
            if (cancellationToken.IsCancellationRequested) return;

            var fullPath = Path.Combine(subFolderPath, file);
            await _fileDownloaderService.DownloadFileAsync(prefix + url, fullPath, browserService);

            await Task.Delay(1000, cancellationToken);
        }

        await browserService.Close();
    }
}