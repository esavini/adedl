using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AdeDl.BlazorApp;

public class AdeDlDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AdeDlDbContext>
{
    public AdeDlDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AdeDlDbContext>();
        
        var sqliteConnectionStringBuilder = new SqliteConnectionStringBuilder();
        sqliteConnectionStringBuilder.DataSource = "database.db";
        sqliteConnectionStringBuilder.ForeignKeys = true;
                
        var connectionString = sqliteConnectionStringBuilder.ToString();
        
        optionsBuilder.UseSqlite(connectionString);

        return new AdeDlDbContext(optionsBuilder.Options);
    }
}