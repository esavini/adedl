using AdeDl.BlazorApp.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace AdeDl.BlazorApp.Services;

public class CredentialService : ICredentialService
{
    private readonly AdeDlDbContext _adeDlDbContext;

    private string? _selectedCredential;

    public CredentialService(AdeDlDbContext adeDlDbContext)
    {
        _adeDlDbContext = adeDlDbContext;
    }

    public Task ChangeSelectedCredentialAsync(string credentialId)
    {
        _selectedCredential = credentialId;
        return Task.CompletedTask;
    }

    public async Task SaveAsync(Credential credential)
    {
        if (credential.Id is null)
        {
            await _adeDlDbContext.Credentials.AddAsync(credential);
        }
        else
        {
            _adeDlDbContext.Credentials.Update(credential);
        }

        await _adeDlDbContext.SaveChangesAsync();
    }

    public async Task DeleteCredentialAsync(Credential credential)
    {
        var c = await _adeDlDbContext.Credentials.FindAsync(credential.Id);

        if (c is null) return;

        _adeDlDbContext.Credentials.Remove(c);
        await _adeDlDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Credential>> ListCredentialsAsync()
    {
        var credentials = await _adeDlDbContext.Credentials
            .ToListAsync();

        return credentials;
    }

    public string? SelectedCredentialId => _selectedCredential;

    public async Task<Credential> GetCurrentCredentialAsync() =>
        await _adeDlDbContext.Credentials.FindAsync(_selectedCredential) ?? throw new Exception();
}