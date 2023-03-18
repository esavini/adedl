using PuppeteerSharp;

namespace AdeDl.BlazorApp.Services
{
    public interface ILoginService
    {
        Task<IBrowserService> LoginAsync();
        
        IEnumerable<CookieParam> Cookies { get; }
    }
}