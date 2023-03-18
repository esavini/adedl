using AdeDl.BlazorApp.Models.Database;

namespace AdeDl.BlazorApp.Services;

public class StateKeeper : IStateKeeper
{
    public async Task SetSelectedCustomers(List<Customer> customers)
    {
        SelectedCustomers = customers;
    }

    public ICollection<Customer> SelectedCustomers { get; private set; } = Array.Empty<Customer>();
}