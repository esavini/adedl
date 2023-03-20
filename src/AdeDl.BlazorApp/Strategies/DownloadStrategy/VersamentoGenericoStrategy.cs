using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;
using AdeDl.BlazorApp.Services;

namespace AdeDl.BlazorApp.Strategies.DownloadStrategy;

public class VersamentoGenericoStrategy : IDownloadStrategy
{
    private readonly IVersamentoGenericoOperationService _versamentoGenericoOperationService;

    public VersamentoGenericoStrategy(IVersamentoGenericoOperationService versamentoGenericoOperationService)
    {
        _versamentoGenericoOperationService = versamentoGenericoOperationService;
    }
    
    public bool CanHandle(IOperation operation) => operation is VersamentoGenericoOperation;

    public async Task DownloadAsync(Customer customer, IOperation operation, CancellationToken cancellationToken)
    {
        await _versamentoGenericoOperationService.DownloadAsync(customer, (VersamentoGenericoOperation)operation, cancellationToken);
    }
}