using PuppeteerSharp;

namespace AdeDl.BlazorApp.Services
{
    public interface IBrowserService
    {
        Task CreateClientAsync(bool visible, Func<IEnumerable<CookieParam>, bool>? doneFunc = null,
            Action? callback = null);

        Task<IEnumerable<CookieParam>> GetCookiesAsync();

        Task GoToAsync(string url, IEnumerable<CookieParam>? cookies = null);

        Task<T> ActAsync<T>(string code);
        
        Task ActAsync(string code);

        Task Close();
        
        Task PdfAsync(string pathRiepilogo);
        
        Task<string> GetCurrentAddress();
    }
}