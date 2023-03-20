using AdeDl.BlazorApp.Models.Cascade;
using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Services;
using Microsoft.AspNetCore.Components;

namespace AdeDl.BlazorApp.Components;

public partial class SelectCustomerRow
{
    [Inject] private ICassettoFiscaleService CassettoFiscaleService { get; set; } = default!;

    [Parameter] public Customer Customer { get; set; } = default!;
    
    [Parameter] public Action<Customer> OnDelete { get; set; } = default!;

    [CascadingParameter(Name = nameof(AddCustomerCascadeModel))]
    public AddCustomerCascadeModel AddCustomerCascadeModel { get; set; } = null!;

    private void OpenCassetto()
    {
        var _ = CassettoFiscaleService.OpenCassettoAsync(Customer);
    }
    
    private void OpenFatture()
    {
        var _ = CassettoFiscaleService.OpenFatturazioneElettronicaAsync(Customer);
    }

    private bool IsSelected()
    {
        return AddCustomerCascadeModel.IsSelected(Customer);
    }

    private void SelectionChanged()
    {
         AddCustomerCascadeModel.ToggleCustomerSelection(Customer);
         StateHasChanged();
    }

    private async Task Delete()
    {
        if (IsSelected())
        {
            AddCustomerCascadeModel.ToggleCustomerSelection(Customer);
        }
        
        OnDelete.Invoke(Customer);
    }
}