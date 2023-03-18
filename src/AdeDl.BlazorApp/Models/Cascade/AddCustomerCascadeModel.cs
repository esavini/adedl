using AdeDl.BlazorApp.Models.Database;

namespace AdeDl.BlazorApp.Models.Cascade;

public class AddCustomerCascadeModel
{
    public void NotifyCustomersChanged()
    {
        CustomersChanged?.Invoke();
    }
    
    public delegate void CustomersChangedEventHandler();
    
    public event CustomersChangedEventHandler? CustomersChanged;
    
    public delegate void CustomersSelectionChangedEventHandler();
    
    public event CustomersSelectionChangedEventHandler? CustomersSelectionChanged;

    public List<Customer> Customers { get; set; } = new();

    public void ToggleCustomerSelection(Customer customer)
    {
        if(Customers.Any(c => c.Id == customer.Id))
        {
            Customers.Remove(customer);
        }
        else
        {
            Customers.Add(customer);
        }
        
        CustomersSelectionChanged?.Invoke();
    }

    public bool IsSelected(Customer customer)
    {
        return Customers.Any(c => c.Id == customer.Id);
    }
}