using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Publisher.DTO.RequestDTO;
using Publisher.DTO.ResponseDTO;
using Publisher.HttpClients.Interfaces;

namespace Publisher.HttpClients.Implementations;

public class CachedDiscussionClient : IDiscussionClient
{
    private readonly IDiscussionClient _innerClient;
    private readonly IDistributedCache _cache;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(2);

    public CachedDiscussionClient(IDiscussionClient innerClient, IDistributedCache cache)
    {
        _innerClient = innerClient;
        _cache = cache;
    }
    
    public async Task<IEnumerable<NoticeResponseDTO>?> GetNoticesAsync()
    {
        const string cacheKey = "discussion:notices_all";
        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<IEnumerable<NoticeResponseDTO>>(cachedData, _jsonOptions);
        }
        
        var notices = await _innerClient.GetNoticesAsync();
        if (notices != null)
        {
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(notices, _jsonOptions),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheDuration
                });
        }
        
        return notices;
    }

    public async Task<NoticeResponseDTO?> GetNoticeByIdAsync(long id)
    {
        string cacheKey = $"discussion:notice:{id}";
        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<NoticeResponseDTO>(cachedData, _jsonOptions);
        }
        
        var notice = await _innerClient.GetNoticeByIdAsync(id);
        if (notice != null)
        {
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(notice, _jsonOptions),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheDuration
                });
        }
        
        return notice;
    }

    public async Task<NoticeResponseDTO?> CreateNoticeAsync(NoticeRequestDTO post)
    {
        var notice = await _innerClient.CreateNoticeAsync(post);
        await InvalidateCacheAsync(notice.Id);
        return notice;
    }

    public async Task<NoticeResponseDTO?> UpdateNoticeAsync(NoticeRequestDTO post)
    {
        var notice = await _innerClient.UpdateNoticeAsync(post);
        await InvalidateCacheAsync(notice.Id);
        return notice;
    }

    public async Task DeleteNoticeAsync(long id)
    {
        await _innerClient.DeleteNoticeAsync(id);
        await InvalidateCacheAsync(id);
    }

    /// <summary>
    /// Инвалидирует кэш для списка и отдельного Notice.
    /// Если у вас есть дополнительные ключи, их тоже можно добавить.
    /// </summary>
    private async Task InvalidateCacheAsync(long id)
    {
        await _cache.RemoveAsync("discussion:notices_all");
        await _cache.RemoveAsync($"discussion:notice:{id}");
    }
}
