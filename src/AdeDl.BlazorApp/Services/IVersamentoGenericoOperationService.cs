using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;

namespace AdeDl.BlazorApp.Services;

public interface IVersamentoGenericoOperationService
{
    Task DownloadAsync(Customer customer, VersamentoGenericoOperation operation, CancellationToken cancellationToken);
}