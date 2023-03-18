using AdeDl.BlazorApp.Models.Database;
using PuppeteerSharp;

namespace AdeDl.App.Services
{
    public interface IF24Service
    {
        Task DownloadF24Async(Customer fiscalCode, IEnumerable<CookieParam> cookies);
    }
}