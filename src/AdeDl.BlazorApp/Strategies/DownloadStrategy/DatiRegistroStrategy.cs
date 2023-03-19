using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;
using AdeDl.BlazorApp.Services;

namespace AdeDl.BlazorApp.Strategies.DownloadStrategy;

public class DatiRegistroStrategy : IDownloadStrategy
{
    private readonly IDatiDelRegistroService _datiDelRegistroService;

    public DatiRegistroStrategy(IDatiDelRegistroService datiDelRegistroService)
    {
        _datiDelRegistroService = datiDelRegistroService;
    }
    
    public bool CanHandle(IOperation operation) => operation is DatiRegistro;

    public async Task DownloadAsync(Customer customer, IOperation operation, CancellationToken cancellationToken)
    {
        await _datiDelRegistroService.DownloadAsync(customer, (DatiRegistro) operation, cancellationToken);
    }
}