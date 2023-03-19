using System.Globalization;
using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;

namespace AdeDl.BlazorApp.Services;

public class DatiDelRegistroService : IDatiDelRegistroService
{
    private readonly ICassettoFiscaleService _cassettoFiscaleService;

    private readonly ICredentialService _credentialService;

    private readonly IFileDownloaderService _fileDownloaderService;

    public DatiDelRegistroService(ICassettoFiscaleService cassettoFiscaleService, ICredentialService credentialService,
        IFileDownloaderService fileDownloaderService)
    {
        _cassettoFiscaleService = cassettoFiscaleService;
        _credentialService = credentialService;
        _fileDownloaderService = fileDownloaderService;
    }

    public async Task DownloadAsync(Customer customer, DatiRegistro operation, CancellationToken cancellationToken)
    {
        var currentCredential = await _credentialService.GetCurrentCredentialAsync();
        var browserService = await _cassettoFiscaleService.OpenRawCassettoAsync(customer, false);

        await Task.Delay(2000, cancellationToken);

        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var subFolderPath = Path.Combine(path, "AdeDl", currentCredential.Name, customer.Name, "Dati del Registro");

        Directory.CreateDirectory(subFolderPath);
        
        await browserService.GoToAsync(
            "https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=ARE&Anno=" +
            operation.Year);
            
        await Task.Delay(1000, cancellationToken);

        const string dateAttiJsExtract =
            @"Array.from(document.querySelectorAll(""table.dati tr[class] td[headers='data']"")).map(s => s.innerText)";
        const string tipoAttiJsExtract =
            @"Array.from(document.querySelectorAll(""table.dati tr[class] td[headers='tipo']"")).map(s => s.innerText)";

        var date = await browserService.ActAsync<string[]>(dateAttiJsExtract);
        var tipi = await browserService.ActAsync<string[]>(tipoAttiJsExtract);

        for (int i = 0, j = 1; i < tipi.Length && j < date.Length; i++, j += 2)
        {
            var data = DateTime.ParseExact(date[j], "d/M/yyyy", CultureInfo.InvariantCulture);

            var filePath = Path.Combine(subFolderPath, $"{data:yyyy.MM.dd} - {tipi[i]}.pdf");

            if (File.Exists(filePath))
            {
                continue;
            }

            var fileUrl =
                $"https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=DetARE&Anno=2021&indice={i}&stampa=S&CF={customer.FiscalCode}";
        
            await _fileDownloaderService.DownloadFileAsync(fileUrl, filePath, browserService);
        }

        await browserService.Close();
    }
}