using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Publisher.Models;
using Publisher.Repositories.Interfaces;

namespace Publisher.Repositories.Implementations;

public class CachedUserRepository : IUserRepository
{
    private readonly IUserRepository _decorated;
    private readonly IDistributedCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(2);

    public CachedUserRepository(IUserRepository decorated, IDistributedCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        const string cacheKey = "users_all";
        var cachedData = await _cache.GetStringAsync(cacheKey);
        
        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<IEnumerable<User>>(cachedData);
        }

        var users = await _decorated.GetAllAsync();
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(users), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _cacheDuration
        });
        
        return users;
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        string cacheKey = $"user_{id}";
        Console.WriteLine(cacheKey);
        var cachedData = await _cache.GetStringAsync(cacheKey);
        
        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<User>(cachedData);
        }

        var user = await _decorated.GetByIdAsync(id);
        if (user != null)
        {
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(user), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            });
        }
        
        return user;
    }

    // Остальные методы с инвалидацией кэша
    public async Task<User> CreateAsync(User entity)
    {
        var result = await _decorated.CreateAsync(entity);
        await InvalidateCacheForUser(result.Id);
        return result;
    }

    public async Task<User?> UpdateAsync(User entity)
    {
        var result = await _decorated.UpdateAsync(entity);
        if (result != null)
        {
            await InvalidateCacheForUser(result.Id);
        }
        return result;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var result = await _decorated.DeleteAsync(id);
        if (result)
        {
            await InvalidateCacheForUser(id);
        }
        return result;
    }

    private async Task InvalidateCacheForUser(long userId)
    {
        await _cache.RemoveAsync($"user_{userId}");
        await _cache.RemoveAsync("users_all");
    }
}