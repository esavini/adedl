using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AdeDl.BlazorApp;

public class AdeDlDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AdeDlDbContext>
{
    public AdeDlDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AdeDlDbContext>();
        optionsBuilder.UseSqlite("Data Source=db.db");

        return new AdeDlDbContext(optionsBuilder.Options);
    }
}