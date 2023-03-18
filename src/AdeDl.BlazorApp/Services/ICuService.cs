using AdeDl.BlazorApp.Models.Database;
using PuppeteerSharp;

namespace AdeDl.App.Services
{
    public interface ICuService
    {
        Task DownloadCuAsync(Customer fiscalCode,  IEnumerable<CookieParam> cookies);
    }
}