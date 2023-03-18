using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;

namespace AdeDl.BlazorApp.Strategies.DownloadStrategy;

public interface IDownloadStrategy
{
    bool CanHandle(IOperation operation);
    
    Task DownloadAsync(Customer customer, IOperation operation, CancellationToken cancellationToken);
}