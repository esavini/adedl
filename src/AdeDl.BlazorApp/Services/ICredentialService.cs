using AdeDl.BlazorApp.Models.Requests;
using AdeDl.BlazorApp.Models.Responses;

namespace AdeDl.BlazorApp.Services;

public interface ICredentialService
{
    Task ChangeSelectedCredentialAsync(string credentialId);

    Task CreateCredentialAsync(CredentialCreateModel credential);

    Task DeleteCredentialAsync(string credentialId);

    Task ChangePasswordAsync(CredentialEditModel credential);

    Task<IEnumerable<CredentialListResponse>> ListCredentialsAsync();
}