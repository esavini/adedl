using AdeDl.BlazorApp.Models.Cascade;
using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Services;
using Microsoft.AspNetCore.Components;

namespace AdeDl.BlazorApp.Components;

public partial class SelectCustomers
{
    [Inject] private ICustomerService CustomerService { get; set; } = null!;
    
    [Inject] private ICredentialService CredentialService { get; set; } = null!;
    
    [Inject] private ICassettoFiscaleService CassettoFiscaleService { get; set; } = null!;
    
    [Inject] private IStateKeeper StateKeeper { get; set; } = null!;
    
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    private ICollection<Customer> _customers = Array.Empty<Customer>();
    
    private AddCustomerCascadeModel _addCustomerCascadeModel = new();

    protected override async Task OnInitializedAsync()
    {
        Sync();
        _addCustomerCascadeModel.CustomersChanged += Sync;
        _addCustomerCascadeModel.CustomersSelectionChanged += SelectionChanged;
    }

    private void SelectionChanged()
    {
        StateHasChanged();
    }

    private async void Sync()
    {
        _customers = await CustomerService.ListCustomersAsync(CredentialService.SelectedCredentialId!);
        StateHasChanged();
    }

    private void Accedi()
    {
        var _ = CassettoFiscaleService.OpenCassettoAsync();
    }

    private async Task Next()
    {
        await StateKeeper.SetSelectedCustomers(_addCustomerCascadeModel.Customers);
        NavigationManager.NavigateTo("actionsPage");
    }
}