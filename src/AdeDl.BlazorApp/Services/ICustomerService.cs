using AdeDl.BlazorApp.Models.Database;

namespace AdeDl.BlazorApp.Services;

public interface ICustomerService
{
    Task SaveAsync(Customer customer);

    Task<ICollection<Customer>> ListCustomersAsync(string credentialId);
    Task DeleteAsync(Customer customer);
}