using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;
using Microsoft.Extensions.Logging;

namespace AdeDl.BlazorApp.Strategies.DownloadStrategy;

public class DownloadContext : IDownloadContext
{
    private readonly IEnumerable<IDownloadStrategy> _strategies;
    
    private readonly ILogger<DownloadContext> _logger;

    public DownloadContext(IEnumerable<IDownloadStrategy> strategies, ILogger<DownloadContext> logger)
    {
        _strategies = strategies;
        _logger = logger;
    }

    public async Task DownloadAsync(Customer customer, IEnumerable<BaseOperation> operations,
        CancellationToken cancellationToken)
    {
        foreach (var operation in operations)
        {
            if (cancellationToken.IsCancellationRequested) return;
            
            var strategy = _strategies.FirstOrDefault(s => s.CanHandle(operation));
            
            if (strategy is null)
            {
                _logger.LogWarning("No strategy found for operation {operation}", operation);
                continue;
            }

            await strategy.DownloadAsync(customer, operation, cancellationToken);
        }
    }
}