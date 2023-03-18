using AdeDl.BlazorApp.Models.Database;

namespace AdeDl.BlazorApp.Services;

public interface ICassettoFiscaleService
{
    public Task OpenCassettoAsync();
    
    public Task OpenCassettoAsync(Customer customer);
    
    public Task OpenFatturazioneElettronicaAsync(Customer customer);
}