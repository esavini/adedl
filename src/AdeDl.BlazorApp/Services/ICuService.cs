using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;
using PuppeteerSharp;

namespace AdeDl.BlazorApp.Services
{
    public interface ICuService
    {
        Task DownloadCuAsync(Customer customer, Cu request, CancellationToken cancellationToken);
    }
}