namespace AdeDl.BlazorApp.Models.View;

public class CredentialSelectionModel
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;
    
    public bool IsCurrentlySelected { get; set; }
}