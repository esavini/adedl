using AdeDl.BlazorApp.Models.Database;

namespace AdeDl.BlazorApp.Services;

public class CassettoFiscaleService : ICassettoFiscaleService
{
    private readonly ILoginService _loginService;

    private readonly ICredentialService _credentialService;

    public CassettoFiscaleService(ILoginService loginService,
        ICredentialService credentialService)
    {
        _loginService = loginService;
        _credentialService = credentialService;
    }


    public async Task OpenCassettoAsync() => await _loginService.LoginAsync();

    public async Task<IBrowserService> OpenRawCassettoAsync(Customer customer, bool visible)
    {
        var credential = await _credentialService.GetCurrentCredentialAsync();

        var page = await _loginService.LoginAsync();

        await page.GoToAsync(
            "https://telematici.agenziaentrate.gov.it/CassettoFiscale/Cassetto/AccessoCassettoClientiServlet");

        await Task.Delay(2500);
        await page.ActAsync(
            $@"document.getElementById(""cfCliente"").value=""{customer.FiscalCode}""");
        await page.ActAsync($@"document.getElementById(""pinC"").value = ""{credential.DelegationPassword}""");
        await page.ActAsync(@"document.querySelectorAll(""input.txt_B_R"")[0].click()");
        
        return page;
    }
    
    public async Task OpenCassettoAsync(Customer customer) => await OpenRawCassettoAsync(customer, true);

    public async Task OpenFatturazioneElettronicaAsync(Customer customer)
    {
        var page = await _loginService.LoginAsync();

        await page.GoToAsync(
            "https://portale.agenziaentrate.gov.it/PortaleWeb/servizi/accessoFatturazione");

        await Task.Delay(2500);
        await page.ActAsync(@"document.querySelectorAll(""button.btn.btn-primary[type='submit']"")[1].click()");
        await Task.Delay(3500);
        
        await page.ActAsync($@"document.getElementById(""ut1a3"").click()");
        await page.ActAsync($@"document.getElementById(""tipo_ut_button"").click()");
        
        await Task.Delay(1500);
        
        await page.ActAsync($@"document.getElementById(""cf_inserito"").value=""{customer.FiscalCode}""");
        await page.ActAsync($@"document.getElementById(""delega_submit"").click()");
        
        await Task.Delay(1500);
        
        await page.ActAsync($@"document.getElementById(""chekc_prosegui"").click()");
        await page.ActAsync(@"document.querySelectorAll(""a[href='https://ivaservizi.agenziaentrate.gov.it/portale/web/guest/home']"")[0].click()");

        await Task.Delay(1000);
        await page.ActAsync($@"document.getElementById(""chiudi_informativa"").click()");
    }
}