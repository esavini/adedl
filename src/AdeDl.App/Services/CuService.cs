using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using AdeDl.App.Exceptions;
using AdeDl.App.Models;
using PuppeteerSharp;

namespace AdeDl.App.Services
{
    public class CuService : ICuService
    {
        private readonly IBrowserService _browserService;

        public CuService(IBrowserService browserService)
        {
            _browserService = browserService;
        }

        public async Task DownloadCuAsync(FiscalEntity fiscalCode, IEnumerable<CookieParam> cookies, string pin)
        {
            if(!fiscalCode.CuYear.HasValue) return;
            
            await _browserService.CreateClientAsync(false);
            await _browserService.GoToAsync(
                "https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/AccessoCassettoClientiServlet",
                cookies);

            await _browserService.ActAsync(
                $@"document.getElementById(""cfCliente"").value=""{fiscalCode.FiscalCode}""");
            await _browserService.ActAsync($@"document.getElementById(""pinC"").value=""{pin}""");
            await _browserService.ActAsync($@"document.querySelectorAll(""input.txt_B_R"")[0].click()");

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var subFolderPath = Path.Combine(path, "AdeDl");

            await Task.Delay(1000);
            
            await _browserService.GoToAsync(
                "https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=CUK&Anno=" +
                fiscalCode.CuYear);
            
            const string jsExists = @"document.querySelectorAll("".errore_diagn"").length";
            var existCheckCount = await _browserService.ActAsync<int>(jsExists);

            if (existCheckCount > 0) throw new NotAvailableException();

            const string jsSelectAllCus =
                @"Array.from(document.querySelectorAll('#colonna1 .dati_bordo .dati_contenuto ul li a:first-child')).map(a => a.href);";
            var cus = await _browserService.ActAsync<string[]>(jsSelectAllCus);

            if (!cus.Any()) return;

            const string jsSelectAllSostituti =
                @"Array.from(document.querySelectorAll('#colonna1 .dati_bordo .dati_contenuto ul li a:last-child')).map(a => a.innerText);";
            var sostituti = await _browserService.ActAsync<string[]>(jsSelectAllSostituti);

            const string jsSelectSostituto = @"document.querySelectorAll("".help > b"")[0].innerText";

            await Task.Delay(2000);

            for (var i = 0; i < cus.Length; i++)
            {
                var cu = cus[i];
                var urlSostituto =
                    $"https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=CFColl&CF={sostituti[i]}";

                await _browserService.GoToAsync(urlSostituto);
                var sostituto = await _browserService.ActAsync<string>(jsSelectSostituto);

                if (sostituto.Length > 30)
                {
                    sostituto = sostituto.Substring(0, 30);
                }                
                
                await Task.Delay(1000);
                
                var newCookies = _browserService.GetCookies();
                var webClient = new WebClient();
                webClient.Headers.Add(HttpRequestHeader.Cookie,
                    string.Join("; ", newCookies.Select(c => c.Name + "=" + c.Value)));
                webClient.Headers.Add(HttpRequestHeader.Accept, "*/*");
                webClient.Headers.Add(HttpRequestHeader.UserAgent,
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");

                var queryString = HttpUtility.ParseQueryString(new Uri(cu).Query);
                var protocollo = queryString["Protocollo"];

                webClient.Headers.Add(HttpRequestHeader.Referer,
                    $"https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=CUK&Anno={fiscalCode.CuYear}&Protocollo={protocollo}");

                var cuPath = Path.Combine(subFolderPath, fiscalCode.Name, "CU",
                    $"CU {(fiscalCode.CuYear + 1).ToString()} anno {fiscalCode.CuYear}");
                Directory.CreateDirectory(cuPath);

                var filePath = Path.Combine(cuPath,
                    $"CU {(fiscalCode.CuYear + 1).ToString()} anno {fiscalCode.CuYear} - {sostituto} - Cassetto Fiscale.pdf");

                if (File.Exists(filePath)) continue;

                webClient.DownloadFile(
                    new Uri(
                        $"https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=CUK&Anno={fiscalCode.CuYear}&Protocollo={protocollo}&CF={fiscalCode.FiscalCode}&stampa=P&Fascicoli=SI&TipoStampa=C"),
                    filePath);

                await Task.Delay(2000);
            }

            await _browserService.Close();
        }
    }
}