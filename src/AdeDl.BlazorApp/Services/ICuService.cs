using AdeDl.BlazorApp.Models.Database;
using PuppeteerSharp;

namespace AdeDl.BlazorApp.Services
{
    public interface ICuService
    {
        Task DownloadCuAsync(Customer fiscalCode,  IEnumerable<CookieParam> cookies);
    }
}