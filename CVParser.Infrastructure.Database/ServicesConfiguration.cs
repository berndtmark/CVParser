using CVParser.Application.Interfaces.UnitOfWork;
using CVParser.Infrastructure.Database.Context;
using CVParser.Infrastructure.Database.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CVParser.Infrastructure.Database;

public static class ServicesConfiguration
{
    /// <summary>Name of the database resource in the Aspire AppHost.</summary>
    public const string ConnectionName = "cvparser-db";

    public static IServiceCollection ConfigureInfrastructureDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnectionString = configuration.GetConnectionString(ConnectionName)
            ?? throw new InvalidOperationException($"{ConnectionName} connection string not found.");

        services.AddSingleton<AuditInterceptor>();
        services.AddDbContext<DataBaseContext>((sp, optionsBuilder) =>
            optionsBuilder
                .UseNpgsql(dbConnectionString)
                .AddInterceptors(sp.GetRequiredService<AuditInterceptor>()));

        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }

    /// <summary>Applies any pending EF migrations. Call once at startup, before the host runs.</summary>
    public static async Task MigrateDatabaseAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();

        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
