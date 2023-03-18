using System.Net;
using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;
using Microsoft.Extensions.Logging;

namespace AdeDl.BlazorApp.Services;

public class F24Service : IF24Service
{
    private readonly ICassettoFiscaleService _cassettoFiscaleService;

    private readonly ICredentialService _credentialService;

    private readonly ILogger<F24Service> _logger;

    public F24Service(ICassettoFiscaleService cassettoFiscaleService, ICredentialService credentialService,
        ILogger<F24Service> logger)
    {
        _cassettoFiscaleService = cassettoFiscaleService;
        _credentialService = credentialService;
        _logger = logger;
    }

    public async Task DownloadF24Async(Customer customer, F24 request, CancellationToken cancellationToken)
    {
        var browserService = await _cassettoFiscaleService.OpenRawCassettoAsync(customer, false);
        var currentCredential = await _credentialService.GetCurrentCredentialAsync();

        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var subFolderPath = Path.Combine(path, "AdeDl", currentCredential.Name);

        await Task.Delay(2000, cancellationToken);
        
        await browserService.GoToAsync(
            "https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=F24&Anno=" +
            request.Year);

        const string jsExists = @"document.querySelectorAll("".errore_diagn"").length";
        var existCheckCount = await browserService.ActAsync<int>(jsExists);

        if (existCheckCount > 0)
        {
            _logger.LogInformation("F24 annualità {@Anno} non disponibile per il cliente {@Customer}", request.Year,
                customer);
            return;
        }

        const string jsSelectAllF24s =
            @"Array.from(document.querySelectorAll(""table.dati:first-child tr td:nth-child(1)"")).map(id => parseInt(id.innerText.trim()))";
        var f24s = await browserService.ActAsync<int[]>(jsSelectAllF24s);

        const string jsSelectNumberF24s =
            @"Array.from(document.querySelectorAll(""table.dati:first-child tr td:nth-child(2)"")).map(id => parseInt(id.innerText.trim()))";
        var numberOfF24s = await browserService.ActAsync<int[]>(jsSelectNumberF24s);

        const string jsSelectDateF24s =
            @"Array.from(document.querySelectorAll(""table.dati:first-child tr td:nth-child(3)"")).map(id => id.innerText.trim())";
        var dateOfF24s = await browserService.ActAsync<string[]>(jsSelectDateF24s);

        const string jsSelectProtoF24s =
            @"Array.from(document.querySelectorAll(""table.dati:first-child tr td:nth-child(5)"")).map(id => id.innerText.trim())";
        var protoOfF24s = await browserService.ActAsync<string[]>(jsSelectProtoF24s);

        if (!f24s.Any()) return;

        var f24Path = Path.Combine(subFolderPath, customer.Name, "F24",
            $"F24 anno {request.Year}");

        for (var i = 0; i < f24s.Length; i++)
        {
            var splittedDate = dateOfF24s[i].Split("/");
            var data = splittedDate[2] + "." + splittedDate[1].PadLeft(2, '0') + "." +
                       splittedDate[0].PadLeft(2, '0');

            await DownloadF24(f24Path, customer.FiscalCode, request.Year, data, f24s[i],
                numberOfF24s[i], protoOfF24s[i], browserService);
        }

        var pathRiepilogo = f24Path +
                            $"/{DateTime.Now.Year}.{DateTime.Now.Month.ToString().PadLeft(2, '0')}.{DateTime.Now.Day.ToString().PadLeft(2, '0')} - Versamenti F24 Cass. Fisc.pdf";
        await browserService.PdfAsync(pathRiepilogo);

        await browserService.Close();
    }

    private async Task DownloadF24(string path, string fiscalCode, int year, string data, int progressivo, int totale,
        string protocollo, IBrowserService browserService)
    {
        string finalPath;
        string finalPathQuietanza;

        Directory.CreateDirectory(path);

        var url = "https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?" +
                  $"Ric=DetF24&Anno={year.ToString()}" +
                  $"&dataDal=&dataAl=&CF={fiscalCode}";

        if (totale == 1)
        {
            url += $"&indice={(progressivo - 1).ToString()}";
            finalPath = Path.Combine(path, data + " - " + progressivo + " - F24.pdf");
            finalPathQuietanza = Path.Combine(path, data + " - " + progressivo + " - F24 Quietanza.pdf");

            if (!File.Exists(finalPath))
            {
                await DownloadFile(url + "&stampa=P", finalPath, browserService);
            }

            if (!File.Exists(finalPathQuietanza))
            {
                await DownloadFile(url + "&stampa=Q", finalPathQuietanza, browserService);
            }

            Task.Delay(2000);
        }
        else
        {
            for (int i = 0; i < totale; i++)
            {
                var protoSplitted = protocollo.Split("/");

                url += $"&indice={i.ToString()}&ProtTelem={protoSplitted[0]}&ProgrTelem={protoSplitted[1]}";

                var dir = new DirectoryInfo(path);

                foreach (var file in dir.EnumerateFiles())
                {
                    if (file.Name.EndsWith("F24 Quietanza.pdf"))
                    {
                        file.Delete();
                    }
                }

                finalPath = Path.Combine(path, data + " - " + progressivo + "." + (i + 1) + " - F24.pdf");
                finalPathQuietanza = Path.Combine(path,
                    data + " - " + progressivo + "." + (i + 1) + " - F24 Quietanza.pdf");

                if (!File.Exists(finalPath))
                {
                    await DownloadFile(url + "&stampa=P", finalPath, browserService);
                }

                if (!File.Exists(finalPathQuietanza))
                {
                    await DownloadFile(url + "&stampa=Q", finalPathQuietanza, browserService);
                }

                Task.Delay(2000);
            }
        }
    }

    private async Task DownloadFile(string url, string path, IBrowserService browserService)
    {
        var newCookies = await browserService.GetCookiesAsync();
        var webClient = new WebClient();
        webClient.Headers.Add(HttpRequestHeader.Cookie,
            string.Join("; ", newCookies.Select(c => c.Name + "=" + c.Value)));
        webClient.Headers.Add(HttpRequestHeader.Accept, "*/*");
        webClient.Headers.Add(HttpRequestHeader.UserAgent,
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");

        webClient.DownloadFile(new Uri(url), path);
    }
}