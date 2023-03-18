using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Services;
using PuppeteerSharp;

namespace AdeDl.App.Services
{
    public class LoginService : ILoginService
    {
        private readonly IBrowserService _browserService;

        private readonly ICredentialService _credentialService;

        public LoginService(IBrowserService browserService, ICredentialService credentialService)
        {
            _browserService = browserService;
            _credentialService = credentialService;
        }

        public async Task<IBrowserService> LoginAsync()
        {
            var credential = await _credentialService.GetCurrentCredentialAsync();

            await _browserService.CreateClientAsync(true);
            await _browserService.GoToAsync("https://iampe.agenziaentrate.gov.it/sam/UI/Login?realm=/agenziaentrate");
            await _browserService.ActAsync(@"document.getElementById(""tab-form"").click()");
            await _browserService.ActAsync(
                @$"document.getElementById('username-fo-ent').value=""{credential.Username}""");
            await _browserService.ActAsync(
                @$"document.getElementById('password-fo-ent').value=""{credential.Password}""");
            await _browserService.ActAsync(@$"document.getElementById('pin-fo-ent').value=""{credential.Pin}""");
            await _browserService.ActAsync(@"document.querySelector(""#tab-fo-ent .btn-accedi"").click()");

            do
            {
                await Task.Delay(1500);
            } while (await _browserService.GetCurrentAddress() != "https://portale.agenziaentrate.gov.it/PortaleWeb/home");

            return _browserService;
        }

        public IEnumerable<CookieParam> Cookies { get; private set; }
    }
}