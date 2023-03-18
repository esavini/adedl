using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Services;
using Microsoft.AspNetCore.Components;

namespace AdeDl.BlazorApp.Components;

public partial class SelectCredentialForm
{
    [Inject] private ICredentialService CredentialService { get; set; } = default!;
    
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private bool IsLoading { get; set; } = true;

    private IEnumerable<Credential> _credentials = Array.Empty<Credential>();

    private Credential? _currentEditingCredential;

    private ElementReference? _credentialsField;
    
    private string? _selectedCredentialId = null;

    protected override async Task OnInitializedAsync() => await Sync();

    private async Task Sync()
    {
        IsLoading = true;
        StateHasChanged();

        _credentials = await CredentialService.ListCredentialsAsync();
        _currentEditingCredential = _credentials.FirstOrDefault();
        _selectedCredentialId = _currentEditingCredential?.Id;

        if (!_credentials.Any())
        {
            _currentEditingCredential = new Credential();
        }

        IsLoading = false;
        StateHasChanged();
    }

    private async Task HandleSubmitAsync()
    {
        if (_currentEditingCredential is null)
        {
            return;
        }

        await CredentialService.SaveAsync(_currentEditingCredential);
        await Sync();
    }
    
    private async Task HandleDeleteAsync()
    {
        if (_currentEditingCredential?.Id is null) return;

        await CredentialService.DeleteCredentialAsync(_currentEditingCredential);
        await Sync();
    }

    private async Task HandleNewCredentialAsync()
    {
        _currentEditingCredential = new Credential();
        StateHasChanged();
    }
    
    private async Task NextAsync()
    {
        if (_selectedCredentialId is null)
        {
            return;
        }

        await CredentialService.ChangeSelectedCredentialAsync(_selectedCredentialId);
        NavigationManager.NavigateTo("/selectCustomers");
    }

    private void SelectCredential(ChangeEventArgs changeEventArgs)
    {
        _selectedCredentialId = changeEventArgs.Value?.ToString();
        _currentEditingCredential = _credentials.FirstOrDefault(c => c.Id == _selectedCredentialId);
        StateHasChanged();
    }
}