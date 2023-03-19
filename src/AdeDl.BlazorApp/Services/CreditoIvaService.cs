using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;

namespace AdeDl.BlazorApp.Services;

public class CreditoIvaService : ICreditoIvaService
{
    private readonly ICassettoFiscaleService _cassettoFiscaleService;

    private readonly ICredentialService _credentialService;

    public CreditoIvaService(ICassettoFiscaleService cassettoFiscaleService, ICredentialService credentialService)
    {
        _cassettoFiscaleService = cassettoFiscaleService;
        _credentialService = credentialService;
    }

    public async Task DownloadCreditoIvaAsync(Customer customer, CreditoIva request,
        CancellationToken cancellationToken)
    {
        var browserService = await _cassettoFiscaleService.OpenRawCassettoAsync(customer, false);
        var currentCredential = await _credentialService.GetCurrentCredentialAsync();

        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var subFolderPath = Path.Combine(path, "AdeDl", currentCredential.Name, customer.Name,
            "Crediti IVA e Agevolazioni");

        await Task.Delay(2000, cancellationToken);

        Directory.CreateDirectory(subFolderPath);

        await browserService.GoToAsync(
            "https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/CassettoFiscaleServlet?Ric=A00&Anno=" +
            request.Year);

        var fileName = $"{DateTime.Now:yyyy.MM.dd} - Credito IVA {request.Year} - Cassetto Fiscale.pdf";

        await browserService.PdfAsync(Path.Combine(subFolderPath, fileName));
        await browserService.Close();
    }
}