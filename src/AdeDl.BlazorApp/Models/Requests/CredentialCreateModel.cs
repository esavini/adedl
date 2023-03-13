using System.ComponentModel.DataAnnotations;

namespace AdeDl.BlazorApp.Models.Requests;

public class CredentialCreateModel
{
    [Required] public string Name { get; set; } = default!;
    
    [Required] public string Username { get; set; } = default!;
    
    [Required] public string Password { get; set; } = default!;
    
    [Required] public string DelegationPassword { get; set; } = default!;
}