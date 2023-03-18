using AdeDl.BlazorApp.Models;
using AdeDl.BlazorApp.Services;
using Microsoft.AspNetCore.Components;

namespace AdeDl.BlazorApp.Components;

public partial class ActionsComponent
{
    [Inject] private IStateKeeper StateKeeper { get; set; } = default!;

    private int _selectedCustomersCount = 0;

    private List<BaseOperation> _operations = new()
    {
        new Anagrafica()
    };

    protected override void OnInitialized()
    {
        _selectedCustomersCount = StateKeeper.SelectedCustomers.Count;
    }

    private bool IsOperationSelected(Type operationType) => _operations.Any(o => o.GetType() == operationType);
}