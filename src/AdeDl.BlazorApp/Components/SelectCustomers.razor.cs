using AdeDl.BlazorApp.Models.Cascade;
using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

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

    private string? _filter;

    private Virtualize<Customer> _virtualize;

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
        var c = await CustomerService.ListCustomersAsync(CredentialService.SelectedCredentialId!);

        _customers = c.Where(cu =>
            {
                if (string.IsNullOrWhiteSpace(_filter)) return true;

                return cu.Name.Contains(_filter, StringComparison.OrdinalIgnoreCase) ||
                       cu.FiscalCode.Contains(_filter, StringComparison.OrdinalIgnoreCase);
            })
            .OrderBy(cu => cu.Name)
            .ToList();
        
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

    private void ChangeFilter(ChangeEventArgs e)
    {
        _filter = e.Value?.ToString()?.Trim();
        Sync();
    }

    private async void DeleteCustomer(Customer c)
    {
        await CustomerService.DeleteAsync(c);
        Sync();
    }
}