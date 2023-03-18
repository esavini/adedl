using AdeDl.BlazorApp.Models.Database;

namespace AdeDl.BlazorApp.Services;

public interface IStateKeeper
{
    Task SetSelectedCustomers(List<Customer> customers);
    
    ICollection<Customer> SelectedCustomers { get; }
}