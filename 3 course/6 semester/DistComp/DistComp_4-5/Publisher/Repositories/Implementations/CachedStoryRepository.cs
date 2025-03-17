// CachedStoryRepository.cs

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Publisher.Models;
using Publisher.Repositories.Interfaces;

namespace Publisher.Repositories.Implementations;

public class CachedStoryRepository : IStoryRepository
{
    private readonly IStoryRepository _decorated;
    private readonly IDistributedCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(2);

    public CachedStoryRepository(IStoryRepository decorated, IDistributedCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public async Task<IEnumerable<Story>> GetAllAsync()
    {
        const string cacheKey = "stories_all";
        var cachedData = await _cache.GetStringAsync(cacheKey);
        
        if (!string.IsNullOrEmpty(cachedData))
            return JsonSerializer.Deserialize<IEnumerable<Story>>(cachedData);

        var stories = await _decorated.GetAllAsync();
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(stories), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _cacheDuration
        });
        
        return stories;
    }

    public async Task<Story?> GetByIdAsync(long id)
    {
        var cacheKey = $"story_{id}";
        var cachedData = await _cache.GetStringAsync(cacheKey);
        
        if (!string.IsNullOrEmpty(cachedData))
            return JsonSerializer.Deserialize<Story>(cachedData);

        var story = await _decorated.GetByIdAsync(id);
        if (story != null)
        {
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(story), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            });
        }
        
        return story;
    }

    public async Task<Story> CreateAsync(Story entity)
    {
        var result = await _decorated.CreateAsync(entity);
        await InvalidateCacheForStory(result.Id);
        return result;
    }

    public async Task<Story?> UpdateAsync(Story entity)
    {
        var result = await _decorated.UpdateAsync(entity);
        if (result != null)
            await InvalidateCacheForStory(result.Id);
        
        return result;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var result = await _decorated.DeleteAsync(id);
        if (result)
            await InvalidateCacheForStory(id);
        
        return result;
    }

    private async Task InvalidateCacheForStory(long storyId)
    {
        await _cache.RemoveAsync($"story_{storyId}");
        await _cache.RemoveAsync("stories_all");
    }
}