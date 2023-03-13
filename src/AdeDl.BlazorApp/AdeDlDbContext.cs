using AdeDl.BlazorApp.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace AdeDl.BlazorApp;

public class AdeDlDbContext : DbContext
{
    public AdeDlDbContext(DbContextOptions<AdeDlDbContext> options) : base(options)
    {
    }

    public DbSet<Credential> Credentials { get; set; } = default!; 
    
}