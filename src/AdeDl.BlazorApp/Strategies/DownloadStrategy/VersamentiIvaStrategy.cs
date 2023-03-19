using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;
using AdeDl.BlazorApp.Services;

namespace AdeDl.BlazorApp.Strategies.DownloadStrategy;

public class VersamentiIvaStrategy : IDownloadStrategy
{
    private readonly IVersamentiIvaService _versamentiIvaService;

    public VersamentiIvaStrategy(IVersamentiIvaService versamentiIvaService)
    {
        _versamentiIvaService = versamentiIvaService;
    }

    public bool CanHandle(IOperation operation) => operation is VersamentiIva;

    public async Task DownloadAsync(Customer customer, IOperation operation, CancellationToken cancellationToken)
    {
        await _versamentiIvaService.DownloadAsync(customer, (VersamentiIva) operation, cancellationToken);
    }
}