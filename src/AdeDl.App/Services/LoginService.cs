using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdeDl.App.Models;
using PuppeteerSharp;

namespace AdeDl.App.Services
{
    public class LoginService : ILoginService
    {
        private readonly IBrowserService _browserService;

        private const string AuthCookieName = "cookieutentee0194";

        public LoginService(IBrowserService browserService)
        {
            _browserService = browserService;
        }

        public async Task<bool> LoginAsync(Credential credentials)
        {
            bool done = false;
            await _browserService.CreateClientAsync(false, VerifyAuthCookiesWasCreated, () => done = true);
            await _browserService.GoToAsync("https://telematici.agenziaentrate.gov.it/Main/login.jsp");
            await _browserService.ActAsync(@$"document.getElementById('nome_utente_ar').value=""{credentials.Username}""");
            await _browserService.ActAsync(@$"document.getElementById('password_ar').value=""{credentials.Password}""");
            await _browserService.ActAsync(@$"document.getElementById('codicepin').value=""{credentials.Pin}""");
            await _browserService.ActAsync(@"document.querySelector(""input[type='submit'].conferma"").click()");
            
            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while(!done) {}
            
            var cookies = _browserService.GetCookies();
            var result = VerifyAuthCookiesWasCreated(cookies);

            Cookies = cookies;

            return result;
        }

        public IEnumerable<CookieParam> Cookies { get; private set; }

        public string FiscalCode { get; private set; }

        private bool VerifyAuthCookiesWasCreated(IEnumerable<CookieParam> cookies) =>
            cookies?.FirstOrDefault(c => c.Name == AuthCookieName)?.Value is not null;
    }
}