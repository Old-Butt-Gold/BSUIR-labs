using Microsoft.EntityFrameworkCore;
using Publisher.Data;
using Publisher.HttpClients;
using Publisher.HttpClients.Implementations;
using Publisher.HttpClients.Interfaces;
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

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IStoryService, StoryService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IDiscussionClient, DiscussionClient>();

        return services;
    }
    
    public static IServiceCollection AddDiscussionClient(this IServiceCollection services)
    {
        services
            .AddHttpClient(nameof(DiscussionClient), 
                client => client.BaseAddress = new Uri("http://localhost:24130/api/v1.0/"));

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("PostgresConnection")));

        return services;
    }
}