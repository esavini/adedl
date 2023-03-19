using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;

namespace AdeDl.BlazorApp.Services;

public class VersamentiIvaService : IVersamentiIvaService
{
    private readonly ICassettoFiscaleService _cassettoFiscaleService;

    private readonly ICredentialService _credentialService;

    public VersamentiIvaService(ICassettoFiscaleService cassettoFiscaleService, ICredentialService credentialService)
    {
        _cassettoFiscaleService = cassettoFiscaleService;
        _credentialService = credentialService;
    }

    public async Task DownloadAsync(Customer customer, VersamentiIva operation, CancellationToken cancellationToken)
    {
        var browserService = await _cassettoFiscaleService.OpenRawCassettoAsync(customer, false);
        var currentCredential = await _credentialService.GetCurrentCredentialAsync();

        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var subFolderPath = Path.Combine(path, "AdeDl", currentCredential.Name, customer.Name,
            "Versamenti IVA");

        await Task.Delay(2000, cancellationToken);

        Directory.CreateDirectory(subFolderPath);

        await browserService.GoToAsync(
            "https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=VUSel");

        await Task.Delay(1500, cancellationToken);

        var exists = await browserService.ActAsync<bool>($"document.getElementById('{operation.Year}') !== null");

        if (!exists || cancellationToken.IsCancellationRequested)
        {
            await browserService.Close();
            return;
        }
        
        await browserService.ActAsync($"document.getElementById('{operation.Year}').querySelector('a').click()");
        
        await Task.Delay(1500, cancellationToken);
        
        await browserService.ActAsync("document.getElementById('tributo').click()");
        await browserService.ActAsync(@"document.querySelector('input[name=""tipoTributo""]').value = '6'");
        await browserService.ActAsync(@"document.querySelector('input.txt_B_R[type=""submit""]').click()");
        
        await Task.Delay(1500, cancellationToken);

        var fileName = $"{DateTime.Now:yyyy.MM.dd} - Versamenti IVA {operation.Year} - Cassetto Fiscale.pdf";
        var fullFileName = Path.Combine(subFolderPath, fileName);
        
        if(File.Exists(fullFileName)) File.Delete(fullFileName);
        
        await browserService.PdfAsync(fullFileName);
        await browserService.Close();
    }
}