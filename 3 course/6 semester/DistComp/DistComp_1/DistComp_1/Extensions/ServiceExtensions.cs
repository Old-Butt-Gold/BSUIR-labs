using DistComp_1.Repositories.Implementations;
using DistComp_1.Repositories.Interfaces;
using DistComp_1.Services.Implementations;
using DistComp_1.Services.Interfaces;

namespace DistComp_1.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        services.AddSingleton<IStoryRepository, InMemoryStoryRepository>();
        services.AddSingleton<ITagRepository, InMemoryTagRepository>();
        services.AddSingleton<INoticeRepository, InMemoryNoticeRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IStoryService, StoryService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<INoticeService, NoticeService>();
        
        return services;
    }
}