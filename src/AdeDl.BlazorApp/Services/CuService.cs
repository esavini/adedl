using System.Web;
using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;
using Microsoft.Extensions.Logging;

namespace AdeDl.BlazorApp.Services;

public class CuService : ICuService
{
    private readonly ICassettoFiscaleService _cassettoFiscaleService;

    private readonly ICredentialService _credentialService;

    private readonly IFileDownloaderService _fileDownloaderService;

    private readonly ILogger<CuService> _logger;

    public CuService(ICassettoFiscaleService cassettoFiscaleService, ICredentialService credentialService,
        IFileDownloaderService fileDownloaderService, ILogger<CuService> logger)
    {
        _cassettoFiscaleService = cassettoFiscaleService;
        _credentialService = credentialService;
        _fileDownloaderService = fileDownloaderService;
        _logger = logger;
    }

    public async Task DownloadCuAsync(Customer customer, Cu request, CancellationToken cancellationToken)
    {
        var browserService = await _cassettoFiscaleService.OpenRawCassettoAsync(customer, false);
        var currentCredential = await _credentialService.GetCurrentCredentialAsync();

        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var subFolderPath = Path.Combine(path, "AdeDl", currentCredential.Name);

        await Task.Delay(2000, cancellationToken);

        await browserService.GoToAsync(
            "https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=CUK&Anno=" +
            (request.Year - 1));

        const string jsExists = @"document.querySelectorAll("".errore_diagn"").length";
        var existCheckCount = await browserService.ActAsync<int>(jsExists);

        if (existCheckCount > 0) return;

        const string jsSelectAllCus =
            @"Array.from(document.querySelectorAll('#colonna1 .dati_bordo .dati_contenuto ul li a:first-child')).map(a => a.href);";
        var cus = await browserService.ActAsync<string[]>(jsSelectAllCus);

        if (!cus.Any()) return;

        const string jsSelectAllSostituti =
            @"Array.from(document.querySelectorAll('#colonna1 .dati_bordo .dati_contenuto ul li a:last-child')).map(a => a.innerText);";
        var sostituti = await browserService.ActAsync<string[]>(jsSelectAllSostituti);

        const string jsSelectSostituto = @"document.querySelectorAll("".help > b"")[0].innerText";

        await Task.Delay(2000, cancellationToken);

        var sosCounter = new Dictionary<string, int>();

        for (var i = 0; i < cus.Length; i++)
        {
            var cu = cus[i];
            var urlSostituto =
                $"https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=CFColl&CF={sostituti[i]}";

            await browserService.GoToAsync(urlSostituto);
            var sostituto = await browserService.ActAsync<string>(jsSelectSostituto);

            if (sostituto.Length > 30)
            {
                sostituto = sostituto[..30];
            }

            var isDuplicated = sostituti.Count(s => s == sostituto) > 1;

            if (isDuplicated && !sosCounter.ContainsKey(sostituto))
            {
                sosCounter[sostituto] = 1;
            }

            await Task.Delay(1000);

            var queryString = HttpUtility.ParseQueryString(new Uri(cu).Query);
            var protocollo = queryString["Protocollo"];
            
            var cuPath = Path.Combine(subFolderPath, customer.Name, "CU",
                $"CU {(request.Year).ToString()} anno {request.Year - 1}");
            Directory.CreateDirectory(cuPath);

            var counterString = string.Empty;

            if (isDuplicated)
            {
                counterString = $" -{sosCounter[sostituto]++}";
            }

            var filePath = Path.Combine(cuPath,
                $"CU {(request.Year).ToString()} anno {request.Year - 1} - {sostituto} - Cassetto Fiscale{counterString}.pdf");

            if (File.Exists(filePath)) continue;

            await _fileDownloaderService.DownloadFileAsync(
                $"https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=CUK&Anno={request.Year - 1}&Protocollo={protocollo}&CF={customer.FiscalCode}&stampa=P&Fascicoli=SI&TipoStampa=C",
                filePath,
                browserService
            );

            await Task.Delay(2000, cancellationToken);
        }

        await browserService.Close();
    }
}