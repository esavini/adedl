using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdeDl.BlazorApp.Models.Database;

public class Credential
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string DelegationPassword { get; set; }
}