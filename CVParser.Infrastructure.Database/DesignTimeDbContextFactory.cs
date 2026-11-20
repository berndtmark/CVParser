using CVParser.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CVParser.Infrastructure.Database;

/// <summary>Used by `dotnet ef` at design time only. The connection string is never opened to generate migrations.</summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataBaseContext>
{
    public DataBaseContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseNpgsql("Host=localhost;Database=cvparser-db")
            .Options;

        return new DataBaseContext(options);
    }
}
