using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdeDl.BlazorApp.Models.Database;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string FiscalCode { get; set; }
    
    [Required]
    public string CredentialId { get; set; }

    public Credential Credential { get; set; }
}