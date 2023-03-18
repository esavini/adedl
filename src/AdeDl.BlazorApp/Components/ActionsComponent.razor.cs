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

    private List<IOperation> _operations = new();

    private bool _isDownloading;

    protected override void OnInitialized()
    {
        _selectedCustomersCount = StateKeeper.SelectedCustomers.Count;
    }

    private bool IsOperationSelected(Type operationType) => _operations.Any(o => o.GetType() == operationType);

    private bool IsYearlySelected<T>(int y) where T : IYearlyOperation =>
        IsOperationSelected(typeof(T)) &&
        _operations.Any(o => o.GetType() == typeof(T) && ((T)o).Year == y);

    private void ChangeYear<T>(ChangeEventArgs e) where T : IYearlyOperation
    {
        var op = _operations.First(o => o.GetType() == typeof(T)) as IYearlyOperation;
        op!.Year = int.Parse(e.Value!.ToString() ?? DateTime.Now.Year.ToString());
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

    private void ToggleYearlyOperation<T>() where T : IYearlyOperation, new()
    {
        if (IsOperationSelected(typeof(T)))
        {
            _operations.Remove(_operations.First(o => o.GetType() == typeof(T)));
        }
        else
        {
            _operations.Add(new T
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