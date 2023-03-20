using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;

namespace AdeDl.BlazorApp.Services;

public class VersamentoGenericoOperationService : IVersamentoGenericoOperationService
{
    private readonly IVersamentoGenericoService _versamentoGenericoService;

    private readonly ICassettoFiscaleService _cassettoFiscaleService;

    private readonly ICredentialService _credentialService;

    public VersamentoGenericoOperationService(IVersamentoGenericoService versamentoGenericoService,
        ICassettoFiscaleService cassettoFiscaleService, ICredentialService credentialService)
    {
        _versamentoGenericoService = versamentoGenericoService;
        _cassettoFiscaleService = cassettoFiscaleService;
        _credentialService = credentialService;
    }

    public async Task DownloadAsync(Customer customer, VersamentoGenericoOperation operation,
        CancellationToken cancellationToken)
    {
        var browserService = await _cassettoFiscaleService.OpenRawCassettoAsync(customer, false);
        var currentCredential = await _credentialService.GetCurrentCredentialAsync();

        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var subFolderPath = Path.Combine(path, "AdeDl", currentCredential.Name, customer.Name,
            "Versamenti");

        var versamento = await _versamentoGenericoService.GetVersamentoGenericoAsync(operation.Id);

        await Task.Delay(1500, cancellationToken);

        if (versamento is null) return;

        if (versamento.PeriodYear is not null)
        {
            await browserService.GoToAsync(
                "https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=VUSel");

            await Task.Delay(1500, cancellationToken);

            var exists = await browserService.ActAsync<bool>($"document.getElementById('{versamento.PeriodYear}') !== null");

            if (!exists || cancellationToken.IsCancellationRequested)
            {
                await browserService.Close();
                return;
            }
        
            await browserService.ActAsync($"document.getElementById('{versamento.PeriodYear}').querySelector('a').click()");
        
            await Task.Delay(1500, cancellationToken);            
            
        }
        else
        {
            await browserService.GoToAsync(
                "https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=F24Sel");
            
            await Task.Delay(1500, cancellationToken);
            
            await browserService.ActAsync($"document.getElementById('dataDal').value = '{versamento.PeriodFrom:dd/MM/yyyy}'");
            await browserService.ActAsync(
                $"document.getElementById('dataAl').value = '{versamento.PeriodTo:dd/MM/yyyy}'");
        }

        if (string.IsNullOrWhiteSpace(versamento.Ente))
        {
            await browserService.ActAsync("document.getElementById('tributo').click()");
            await browserService.ActAsync($@"document.querySelector('input[name=""tipoTributo""]').value = '{versamento.Prefisso}'");
            await browserService.ActAsync($@"document.querySelector('input[name=""tipoTributo0""]').value = '{versamento.CodiceTributo1}'");
            await browserService.ActAsync($@"document.querySelector('input[name=""tipoTributo1""]').value = '{versamento.CodiceTributo2}'");
            await browserService.ActAsync($@"document.querySelector('input[name=""tipoTributo2""]').value = '{versamento.CodiceTributo3}'");
            await browserService.ActAsync($@"document.querySelector('input[name=""tipoTributo3""]').value = '{versamento.CodiceTributo4}'");

        }
        else
        {
            await browserService.ActAsync($"document.getElementById('percipiente').value = '{versamento.Ente}'");
        }

        if (versamento.Credito)
        {
            await browserService.ActAsync(@"document.querySelector('input[name=""Tipo""][value=""C""]').click()");
        }
        
        if (versamento.Coobbligato)
        {
            await browserService.ActAsync(@"document.querySelector('input[name=""tipoRicerca""][value=""C""]').click()");
        }
        
        if (versamento.NoAddizionale)
        {
            await browserService.ActAsync(@"document.querySelector('#ACC').click()");
        }
        
        await browserService.ActAsync(@"document.querySelector('input.txt_B_R[type=""submit""]').click()");
        
        await Task.Delay(1500, cancellationToken);

        Directory.CreateDirectory(subFolderPath);
        
        var fileName = $"{DateTime.Now:yyyy.MM.dd} - {versamento.Name} - Cassetto Fiscale.pdf";
        var fullFileName = Path.Combine(subFolderPath, fileName);

        if(File.Exists(fullFileName)) File.Delete(fullFileName);
        
        await browserService.PdfAsync(fullFileName);
        await browserService.Close();
    }
}