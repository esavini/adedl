using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;
using AdeDl.BlazorApp.Services;

namespace AdeDl.BlazorApp.Strategies.DownloadStrategy;

public class AnagraficaStrategy : IDownloadStrategy
{
    private readonly IAnagraficaService _anagraficaService;

    public AnagraficaStrategy(IAnagraficaService anagraficaService)
    {
        _anagraficaService = anagraficaService;
    }
    
    public bool CanHandle(IOperation operation) => operation is Anagrafica;

    public async Task DownloadAsync(Customer customer, IOperation operation, CancellationToken cancellationToken)
    {
        await _anagraficaService.DownloadAnagraficaAsync(customer, (Anagrafica)operation, cancellationToken);
    }
}