using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;

namespace AdeDl.BlazorApp.Services
{
    public interface IF24Service
    {
        Task DownloadF24Async(Customer customer, F24 request, CancellationToken cancellationToken);
    }
}