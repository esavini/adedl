using AdeDl.BlazorApp.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace AdeDl.BlazorApp.Services;

public class CustomerService : ICustomerService
{
    private readonly AdeDlDbContext _adeDlDbContext;

    public CustomerService(AdeDlDbContext adeDlDbContext)
    {
        _adeDlDbContext = adeDlDbContext;
    }

    public async Task SaveAsync(Customer customer)
    {
        if (customer.Id is null)
        {
            await _adeDlDbContext.AddAsync(customer);
        }
        else
        {
            _adeDlDbContext.Update(customer);
        }

        await _adeDlDbContext.SaveChangesAsync();
    }
    
    public Task DeleteAsync(Customer customer)
    {
        _adeDlDbContext.Remove(customer);
        return _adeDlDbContext.SaveChangesAsync();
    }

    public async Task<ICollection<Customer>> ListCustomersAsync(string credentialId) =>
        await _adeDlDbContext.Customers
            .Where(customer => customer.CredentialId == credentialId)
            .ToListAsync();
}