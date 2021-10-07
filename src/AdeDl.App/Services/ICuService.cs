using System.Collections.Generic;
using System.Threading.Tasks;
using AdeDl.App.Models;
using PuppeteerSharp;

namespace AdeDl.App.Services
{
    public interface ICuService
    {
        Task DownloadCuAsync(FiscalEntity fiscalCode,  IEnumerable<CookieParam> cookies, string pin);
    }
}