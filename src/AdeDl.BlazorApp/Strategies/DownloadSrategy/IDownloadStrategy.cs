using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;

namespace AdeDl.BlazorApp.Strategies.DownloadSrategy;

public interface IDownloadStrategy
{
    bool CanHandle(BaseOperation operation);
    
    Task DownloadAsync(Customer customer, BaseOperation operation, CancellationToken cancellationToken);
}