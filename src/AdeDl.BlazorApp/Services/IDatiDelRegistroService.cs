using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;

namespace AdeDl.BlazorApp.Services;

public interface IDatiDelRegistroService
{
    Task DownloadAsync(Customer customer, DatiRegistro operation, CancellationToken cancellationToken);
}