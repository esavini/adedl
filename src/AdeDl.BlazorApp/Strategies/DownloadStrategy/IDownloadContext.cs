using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;

namespace AdeDl.BlazorApp.Strategies.DownloadStrategy;

public interface IDownloadContext
{
    Task DownloadAsync(Customer customer, IEnumerable<BaseOperation> operations, CancellationToken cancellationToken);
}