using AdeDl.BlazorApp.Models.Cascade;
using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Services;
using Microsoft.AspNetCore.Components;

namespace AdeDl.BlazorApp.Components;

public partial class AddCustomerForm
{
    [Inject] private ICredentialService CredentialService { get; set; } = null!;
    
    [Inject] private ICustomerService CustomerService { get; set; } = null!;
    
    [CascadingParameter(Name = nameof(AddCustomerCascadeModel))] public AddCustomerCascadeModel AddCustomerCascadeModel { get; set; } = null!;
    
    private Customer _customer = new();
    
    private bool IsLoading { get; set; }

    protected override void OnInitialized()
    {
        _customer.CredentialId = CredentialService.SelectedCredentialId!;
    }

    private async Task Submit()
    {
        IsLoading = true;
        StateHasChanged();
        
        await CustomerService.SaveAsync(_customer);
        
        _customer = new()
        {
            CredentialId = CredentialService.SelectedCredentialId!
        };

        AddCustomerCascadeModel.NotifyCustomersChanged();

        IsLoading = false;
        StateHasChanged();
    }
}