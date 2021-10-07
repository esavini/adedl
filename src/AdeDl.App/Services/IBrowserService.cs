using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace AdeDl.App.Services
{
    public interface IBrowserService
    {
        Task CreateClientAsync(bool visible, Func<IEnumerable<CookieParam>, bool> doneFunc = null,
            Action callback = null);

        IEnumerable<CookieParam> GetCookies();

        Task GoToAsync(string url, IEnumerable<CookieParam> cookies = null);

        Task<T> ActAsync<T>(string code);
        
        Task ActAsync(string code);

        Task Close();
        
        Task PdfAsync(string pathRiepilogo);
    }
}