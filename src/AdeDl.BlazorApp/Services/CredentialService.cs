using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Requests;
using AdeDl.BlazorApp.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace AdeDl.BlazorApp.Services;

public class CredentialService : ICredentialService
{
    private readonly AdeDlDbContext _adeDlDbContext;

    private string? _selectedCredential = null;

    public CredentialService(AdeDlDbContext adeDlDbContext)
    {
        _adeDlDbContext = adeDlDbContext;
    }
    
    public Task ChangeSelectedCredentialAsync(string credentialId)
    {
        throw new NotImplementedException();
    }

    public async Task CreateCredentialAsync(CredentialCreateModel credential)
    {
        var newCredential = new Credential
        {
            Username = credential.Username,
            Password = credential.Password,
            Name = credential.Name,
            DelegationPassword = credential.DelegationPassword,
        };

        await _adeDlDbContext.Credentials.AddAsync(newCredential);
        await _adeDlDbContext.SaveChangesAsync();
    }

    public Task DeleteCredentialAsync(string credentialId)
    {
        throw new NotImplementedException();
    }

    public Task ChangePasswordAsync(CredentialEditModel credential)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CredentialListResponse>> ListCredentialsAsync()
    {
        var credentials = await _adeDlDbContext.Credentials.Select(c => new CredentialListResponse
        {
            IsCurrentlySelected = _selectedCredential == c.Id,
            Name = c.Name,
            Id = c.Id
        })
            .ToListAsync();

        if (_selectedCredential is null && credentials.Any())
        {
            var selectedCredential = credentials.First();
            _selectedCredential = selectedCredential.Id;
            selectedCredential.IsCurrentlySelected = true;
        }

        return credentials;
    }
}