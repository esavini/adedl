using AdeDl.BlazorApp.Models.Database;
using PuppeteerSharp;
using IBrowser = PuppeteerSharp.IBrowser;

namespace AdeDl.App.Services
{
    public interface ILoginService
    {
        Task<IBrowserService> LoginAsync();
        
        IEnumerable<CookieParam> Cookies { get; }
    }
}