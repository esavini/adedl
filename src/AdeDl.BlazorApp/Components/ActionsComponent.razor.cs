using AdeDl.BlazorApp.Models;
using AdeDl.BlazorApp.Models.Operations;
using AdeDl.BlazorApp.Services;
using AdeDl.BlazorApp.Strategies.DownloadStrategy;
using Microsoft.AspNetCore.Components;

namespace AdeDl.BlazorApp.Components;

public partial class ActionsComponent
{
    [Inject] private IStateKeeper StateKeeper { get; set; } = default!;
    
    [Inject] private IDownloadContext DownloadContext { get; set; } = default!;

    private int _selectedCustomersCount = 0;

    private List<BaseOperation> _operations = new();

    private bool _isDownloading;

    protected override void OnInitialized()
    {
        _selectedCustomersCount = StateKeeper.SelectedCustomers.Count;
    }

    private bool IsOperationSelected(Type operationType) => _operations.Any(o => o.GetType() == operationType);

    private void ToggleF24()
    {
        if (IsOperationSelected(typeof(F24)))
        {
            _operations.Remove(_operations.First(o => o.GetType() == typeof(F24)));
        }
        else
        {
            _operations.Add(new F24
            {
                Year = DateTime.Now.Year
            });
        }

        StateHasChanged();
    }

    private bool IsCuSelected(int y) =>
        IsOperationSelected(typeof(Cu)) &&
        _operations.Any(o => o.GetType() == typeof(Cu) && ((Cu)o).Year == y);
    
    private bool IsF24Selected(int y) =>
        IsOperationSelected(typeof(F24)) &&
        _operations.Any(o => o.GetType() == typeof(F24) && ((F24)o).Year == y);
    
    private void ChangeCuYear(ChangeEventArgs e)
    {
        var cu = _operations.First(o => o.GetType() == typeof(Cu)) as Cu;
        cu!.Year = int.Parse(e.Value!.ToString() ?? DateTime.Now.Year.ToString());
        StateHasChanged();
    }
    
    private void ChangeF24Year(ChangeEventArgs e)
    {
        var f24 = _operations.First(o => o.GetType() == typeof(F24)) as F24;
        f24!.Year = int.Parse(e.Value!.ToString() ?? DateTime.Now.Year.ToString());
        StateHasChanged();
    }

    private void ToggleAnagrafica()
    {
        if (IsOperationSelected(typeof(Anagrafica)))
        {
            _operations.Remove(_operations.First(o => o.GetType() == typeof(Anagrafica)));
        }
        else
        {
            _operations.Add(new Anagrafica());
        }
        
        StateHasChanged();
    }
    
    private void ToggleCu()
    {
        if (IsOperationSelected(typeof(Cu)))
        {
            _operations.Remove(_operations.First(o => o.GetType() == typeof(Cu)));
        }
        else
        {
            _operations.Add(new Cu
            {
                Year = DateTime.Now.Year
            });
        }

        StateHasChanged();
    }

    private CancellationTokenSource? _cts;
    
    private async void Download()
    {
        _isDownloading = true;
        _cts = new CancellationTokenSource();
        
        StateHasChanged();

        foreach (var customer in StateKeeper.SelectedCustomers)
        {

            if (_cts.IsCancellationRequested) continue;

            await DownloadContext.DownloadAsync(customer, _operations, _cts.Token);
        }
    }
    
    private void Abort()
    {
        _cts?.Cancel();
        StateHasChanged();
    }
}