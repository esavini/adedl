using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdeDl.BlazorApp.Models.Database;

public class Credential
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public string Pin { get; set; }
    
    [Required]
    public string DelegationPassword { get; set; }

    public IEnumerable<Customer> Customers { get; }
}