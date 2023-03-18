using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;

namespace AdeDl.BlazorApp.Services;

public interface IAnagraficaService
{
    Task DownloadAnagraficaAsync(Customer customer, Anagrafica operation, CancellationToken cancellationToken);
}