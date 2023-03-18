using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;
using AdeDl.BlazorApp.Services;

namespace AdeDl.BlazorApp.Strategies.DownloadStrategy;

public class CreditoIvaStrategy : IDownloadStrategy
{
    private readonly ICreditoIvaService _creditoIvaService;

    public CreditoIvaStrategy(ICreditoIvaService creditoIvaService)
    {
        _creditoIvaService = creditoIvaService;
    }
    
    public bool CanHandle(IOperation operation) => operation is CreditoIva;

    public async Task DownloadAsync(Customer customer, IOperation operation, CancellationToken cancellationToken)
    {
        await _creditoIvaService.DownloadCreditoIvaAsync(customer, (CreditoIva)operation, cancellationToken);
    }
}