using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;

namespace AdeDl.BlazorApp.Services;

public interface ICreditoIvaService
{
    Task DownloadCreditoIvaAsync(Customer customer, CreditoIva request, CancellationToken cancellationToken);
}