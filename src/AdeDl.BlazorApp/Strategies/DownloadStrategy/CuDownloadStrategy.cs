using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;
using AdeDl.BlazorApp.Services;

namespace AdeDl.BlazorApp.Strategies.DownloadStrategy;

public class CuDownloadStrategy : IDownloadStrategy
{
    private readonly ICuService _cuService;

    public CuDownloadStrategy(ICuService cuService)
    {
        _cuService = cuService;
    }
    
    public bool CanHandle(BaseOperation operation) => operation is Cu;

    public async Task DownloadAsync(Customer customer, BaseOperation operation, CancellationToken cancellationToken)
    {
        await _cuService.DownloadCuAsync(customer, (Cu)operation, cancellationToken);
    }
}