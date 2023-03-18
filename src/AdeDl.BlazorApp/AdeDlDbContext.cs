using AdeDl.BlazorApp.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace AdeDl.BlazorApp;

public class AdeDlDbContext : DbContext
{
    public AdeDlDbContext(DbContextOptions<AdeDlDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Credential>()
            .HasMany<Customer>(p => p.Customers)
            .WithOne(p => p.Credential)
            .HasForeignKey(p => p.CredentialId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Credential> Credentials { get; set; } = default!; 
    
    public DbSet<Customer> Customers { get; set; } = default!;
}