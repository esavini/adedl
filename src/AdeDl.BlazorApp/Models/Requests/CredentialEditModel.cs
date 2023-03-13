using System.ComponentModel.DataAnnotations;

namespace AdeDl.BlazorApp.Models.Requests;

public class CredentialEditModel : CredentialCreateModel
{
    [Required] public string Id { get; set; } = default!;
}