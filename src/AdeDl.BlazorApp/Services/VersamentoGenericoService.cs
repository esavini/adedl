using AdeDl.BlazorApp.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace AdeDl.BlazorApp.Services;

public class VersamentoGenericoService : IVersamentoGenericoService
{
    private readonly AdeDlDbContext _adeDlDbContext;

    public VersamentoGenericoService(AdeDlDbContext adeDlDbContext)
    {
        _adeDlDbContext = adeDlDbContext;
    }
    
    public async Task<VersamentoGenerico?> GetVersamentoGenericoAsync(string id)
    {
        return await _adeDlDbContext.Versamenti.FindAsync(id);
    }

    public async Task SaveAsync(VersamentoGenerico versamentoGenerico)
    {
        if (versamentoGenerico.Id is null)
        {
            await _adeDlDbContext.AddAsync(versamentoGenerico);
        }
        else
        {
            _adeDlDbContext.Versamenti.Update(versamentoGenerico);
        }
        
        await _adeDlDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(VersamentoGenerico versamentoGenerico)
    {
        _adeDlDbContext.Versamenti.Remove(versamentoGenerico);
        await _adeDlDbContext.SaveChangesAsync();
    }

    public async Task<ICollection<VersamentoGenerico>> GetAll()
    {
        return await _adeDlDbContext.Versamenti.ToListAsync();
    }
}