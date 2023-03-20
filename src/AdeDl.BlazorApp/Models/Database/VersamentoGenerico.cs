using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdeDl.BlazorApp.Models.Database;

public class VersamentoGenerico
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    public int? PeriodYear { get; set; }
    
    public DateTime? PeriodFrom { get; set; }
    
    public DateTime? PeriodTo { get; set; }
    
    public string? Ente { get; set; }
    
    public string? CodiceTributo1 { get; set; }
    
    public string? CodiceTributo2 { get; set; }
    
    public string? CodiceTributo3 { get; set; }
    
    public string? CodiceTributo4 { get; set; }
    
    public string? Prefisso { get; set; }
    
    public bool Credito { get; set; }
    
    public bool NoAddizionale { get; set; }
    
    public bool Coobbligato { get; set; }
}