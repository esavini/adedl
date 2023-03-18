using PuppeteerSharp;

namespace AdeDl.BlazorApp.Services
{
    public interface ILoginService
    {
        Task<IBrowserService> LoginAsync(bool visible);
        
        IEnumerable<CookieParam> Cookies { get; }
    }
}