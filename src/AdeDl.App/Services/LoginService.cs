using System;
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
            await _browserService.GoToAsync("https://iampe.agenziaentrate.gov.it/sam/UI/Login?realm=/agenziaentrate");
            await _browserService.ActAsync(@"document.getElementById(""tab-form"").click()");
            await _browserService.ActAsync(
                @$"document.getElementById('username-fo-ent').value=""{credentials.Username}""");
            await _browserService.ActAsync(
                @$"document.getElementById('password-fo-ent').value=""{credentials.Password}""");
            await _browserService.ActAsync(@$"document.getElementById('pin-fo-ent').value=""{credentials.Pin}""");
            await _browserService.ActAsync(@"document.querySelector(""#tab-fo-ent .btn-accedi"").click()");
            
            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (!done)
            {
            }

            var cookies = _browserService.GetCookies();
            var result = VerifyAuthCookiesWasCreated(cookies);

            Cookies = cookies;

            return result;
        }

        public IEnumerable<CookieParam> Cookies { get; private set; }

        public string FiscalCode { get; private set; }

        private bool VerifyAuthCookiesWasCreated(IEnumerable<CookieParam> cookies)
        {
            return cookies?.FirstOrDefault(c => c.Name == AuthCookieName)?.Value is not null;
        }
    }
}