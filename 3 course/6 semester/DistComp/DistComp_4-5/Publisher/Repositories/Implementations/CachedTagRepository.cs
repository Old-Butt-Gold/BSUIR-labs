// CachedTagRepository.cs

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Publisher.Models;
using Publisher.Repositories.Interfaces;

namespace Publisher.Repositories.Implementations;

public class CachedTagRepository : ITagRepository
{
    private readonly ITagRepository _decorated;
    private readonly IDistributedCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(2);

    public CachedTagRepository(ITagRepository decorated, IDistributedCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        const string cacheKey = "tags_all";
        var cachedData = await _cache.GetStringAsync(cacheKey);
        
        if (!string.IsNullOrEmpty(cachedData))
            return JsonSerializer.Deserialize<IEnumerable<Tag>>(cachedData);

        var tags = await _decorated.GetAllAsync();
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(tags), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _cacheDuration
        });
        
        return tags;
    }

    public async Task<Tag?> GetByIdAsync(long id)
    {
        var cacheKey = $"tag_{id}";
        var cachedData = await _cache.GetStringAsync(cacheKey);
        
        if (!string.IsNullOrEmpty(cachedData))
            return JsonSerializer.Deserialize<Tag>(cachedData);

        var tag = await _decorated.GetByIdAsync(id);
        if (tag != null)
        {
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(tag), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            });
        }
        
        return tag;
    }

    public async Task<Tag> CreateAsync(Tag entity)
    {
        var result = await _decorated.CreateAsync(entity);
        await InvalidateCacheForTag(result.Id);
        return result;
    }

    public async Task<Tag?> UpdateAsync(Tag entity)
    {
        var result = await _decorated.UpdateAsync(entity);
        if (result != null)
            await InvalidateCacheForTag(result.Id);
        
        return result;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var result = await _decorated.DeleteAsync(id);
        if (result)
            await InvalidateCacheForTag(id);
        
        return result;
    }

    private async Task InvalidateCacheForTag(long tagId)
    {
        await _cache.RemoveAsync($"tag_{tagId}");
        await _cache.RemoveAsync("tags_all");
    }
}