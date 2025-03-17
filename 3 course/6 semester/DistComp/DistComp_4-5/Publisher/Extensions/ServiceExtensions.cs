using Microsoft.EntityFrameworkCore;
using Publisher.Data;
using Publisher.HttpClients;
using Publisher.HttpClients.Implementations;
using Publisher.HttpClients.Interfaces;
using Publisher.Repositories;
using Publisher.Repositories.Implementations;
using Publisher.Repositories.Interfaces;
using Publisher.Services.Implementations;
using Publisher.Services.Interfaces;

namespace Publisher.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, DatabaseUserRepository>();
        services.AddScoped<IStoryRepository, DatabaseStoryRepository>();
        services.AddScoped<ITagRepository, DatabaseTagRepository>();
        
        // Download Scrutor to Decorate
        services.Decorate<IUserRepository, CachedUserRepository>();
        services.Decorate<IStoryRepository, CachedStoryRepository>();
        services.Decorate<ITagRepository, CachedTagRepository>();
        
        /*
        services.AddScoped<DatabaseUserRepository>();
        services.AddScoped<IUserRepository>(sp =>
        {
            var innerRepo = sp.GetRequiredService<DatabaseUserRepository>();
            var cache = sp.GetRequiredService<IDistributedCache>();
            return new CachedUserRepository(innerRepo, cache);
        });
        */
        
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IStoryService, StoryService>();
        services.AddScoped<ITagService, TagService>();
        
        services.AddScoped<IDiscussionClient, DiscussionClient>();
        services.Decorate<IDiscussionClient, CachedDiscussionClient>();

        return services;
    }
    
    public static IServiceCollection AddDiscussionClient(this IServiceCollection services)
    {
        services
            .AddHttpClient(nameof(DiscussionClient), 
                client => client.BaseAddress = new Uri
                    ($"http://{Environment.GetEnvironmentVariable("DISCUSSION_HOST")}:24130/api/v1.0/"));

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = $"Host={Environment.GetEnvironmentVariable("POSTGRES_HOST")};" +
                               $"Port={Environment.GetEnvironmentVariable("POSTGRES_PORT")};" +
                               $"Database=distcomp;" +
                               $"Username=postgres;" +
                               $"Password=postgres";
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
    
    public static void ApplyMigrations(this IApplicationBuilder app, IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var migrations = db.Database.GetPendingMigrations();

        if (migrations.Any())
        {
            db.Database.Migrate();
        }
    }
}