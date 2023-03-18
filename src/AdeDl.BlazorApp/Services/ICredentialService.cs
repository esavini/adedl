using AdeDl.BlazorApp.Models.Database;

namespace AdeDl.BlazorApp.Services;

public interface ICredentialService
{
    Task ChangeSelectedCredentialAsync(string credentialId);

    Task SaveAsync(Credential credential);

    Task DeleteCredentialAsync(Credential credential);

    Task<IEnumerable<Credential>> ListCredentialsAsync();
    
    string? SelectedCredentialId { get; }
    
    Task<Credential> GetCurrentCredentialAsync();
}