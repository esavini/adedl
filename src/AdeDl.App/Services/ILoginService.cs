using System.Collections.Generic;
using System.Threading.Tasks;
using AdeDl.App.Models;
using PuppeteerSharp;

namespace AdeDl.App.Services
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(Credential credential);
        
        IEnumerable<CookieParam> Cookies { get; }

        string FiscalCode { get; }
    }
}