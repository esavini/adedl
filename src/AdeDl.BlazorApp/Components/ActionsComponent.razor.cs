using System.Collections;
using AdeDl.BlazorApp.Models;
using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;
using AdeDl.BlazorApp.Services;
using AdeDl.BlazorApp.Strategies.DownloadStrategy;
using Microsoft.AspNetCore.Components;

namespace AdeDl.BlazorApp.Components;

public partial class ActionsComponent
{
    [Inject] private IStateKeeper StateKeeper { get; set; } = default!;

    [Inject] private IDownloadContext DownloadContext { get; set; } = default!;

    [Inject] private IVersamentoGenericoService VersamentoGenericoService { get; set; } = default!;

    private int _selectedCustomersCount = 0;

    private List<IOperation> _operations = new();

    private bool _isDownloading;

    private ICollection<VersamentoGenerico>? _versamentiGenerici;

    private VersamentoGenerico? _versamentoGenericoInModifica;

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

    private void ToggleVersamentoGenericoPeriodType(ChangeEventArgs e)
    {
        if (e.Value is null or "0")
        {
            _versamentoGenericoInModifica!.PeriodYear = DateTime.Now.Year;
            _versamentoGenericoInModifica!.PeriodFrom = null;
            _versamentoGenericoInModifica!.PeriodTo = null;
        }
        else
        {
            _versamentoGenericoInModifica!.PeriodYear = null;
            _versamentoGenericoInModifica!.PeriodFrom = new DateTime(DateTime.Now.Year, 1, 1);
            _versamentoGenericoInModifica!.PeriodTo = new DateTime(DateTime.Now.Year, 12, 31);
        }
    }

    private async Task Submit()
    {
        if (_versamentoGenericoInModifica is null) return;

        await VersamentoGenericoService.SaveAsync(_versamentoGenericoInModifica);

        _versamentoGenericoInModifica = null;
        _versamentiGenerici = await VersamentoGenericoService.GetAll();
        StateHasChanged();
    }

    private async Task ToggleMenuVersamentiGenerici()
    {
        if (_versamentiGenerici is null)
            _versamentiGenerici = await VersamentoGenericoService.GetAll();

        else
            _versamentiGenerici = null;

        StateHasChanged();
    }

    private void NewVersamento()
    {
        _versamentoGenericoInModifica = new VersamentoGenerico
        {
            PeriodYear = DateTime.Now.Year
        };

        StateHasChanged();
    }

    private async Task EditVersamento(VersamentoGenerico v)
    {
        _versamentoGenericoInModifica = v;
        _versamentiGenerici = await VersamentoGenericoService.GetAll();
        StateHasChanged();
    }

    private async Task DeleteVersamento(VersamentoGenerico v)
    {
        _operations.RemoveAll(o =>
            o.GetType() == typeof(VersamentoGenericoOperation)
            && ((VersamentoGenericoOperation)o).Id == v.Id
        );

        await VersamentoGenericoService.DeleteAsync(v);

        if (_versamentoGenericoInModifica is not null && _versamentoGenericoInModifica.Id == v.Id)
            _versamentoGenericoInModifica = null;

        _versamentiGenerici = await VersamentoGenericoService.GetAll();

        StateHasChanged();
    }

    private void ToggleVersamentoGenerico(VersamentoGenerico v)
    {
        if (IsVersamentoGenericoChecked(v))
        {
            _operations.RemoveAll(o =>
                o.GetType() == typeof(VersamentoGenericoOperation)
                && ((VersamentoGenericoOperation)o).Id == v.Id
            );
        }
        else
        {
            _operations.Add(new VersamentoGenericoOperation
            {
                Id = v.Id,
            });
        }

        StateHasChanged();
    }

    private bool IsVersamentoGenericoChecked(VersamentoGenerico v)
    {
        return _operations.Any(o =>
            o.GetType() == typeof(VersamentoGenericoOperation)
            && ((VersamentoGenericoOperation)o).Id == v.Id
        );
    }
}