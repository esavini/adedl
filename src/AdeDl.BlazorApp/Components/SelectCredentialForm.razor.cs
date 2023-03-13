using AdeDl.BlazorApp.Models.Responses;
using AdeDl.BlazorApp.Services;
using Microsoft.AspNetCore.Components;

namespace AdeDl.BlazorApp.Components;

public partial class SelectCredentialForm
{
    [Inject] private ICredentialService CredentialService { get; set; }

    private bool IsLoading { get; set; } = true;

    private IEnumerable<CredentialListResponse> _credentials = Array.Empty<CredentialListResponse>();

    protected override async Task OnInitializedAsync()
    {
        var credentials = await CredentialService.ListCredentialsAsync();
    }
}