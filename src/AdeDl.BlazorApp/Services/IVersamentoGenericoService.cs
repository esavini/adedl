using AdeDl.BlazorApp.Models.Database;

namespace AdeDl.BlazorApp.Services;

public interface IVersamentoGenericoService
{
    Task<VersamentoGenerico?> GetVersamentoGenericoAsync(string id);
    
    Task SaveAsync(VersamentoGenerico versamentoGenerico);
    
    Task DeleteAsync(VersamentoGenerico versamentoGenerico);
    
    Task<ICollection<VersamentoGenerico>> GetAll();
}