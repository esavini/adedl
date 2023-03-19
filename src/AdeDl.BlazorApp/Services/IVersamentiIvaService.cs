using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;

namespace AdeDl.BlazorApp.Services;

public interface IVersamentiIvaService
{
    Task DownloadAsync(Customer customer, VersamentiIva operation, CancellationToken cancellationToken);
}