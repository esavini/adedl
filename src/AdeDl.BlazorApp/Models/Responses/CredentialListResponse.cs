namespace AdeDl.BlazorApp.Models.Responses;

public class CredentialListResponse
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;
    
    public bool IsCurrentlySelected { get; set; }
}