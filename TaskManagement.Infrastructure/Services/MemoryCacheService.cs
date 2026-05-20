using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.Services;

public class MemoryCacheService : ICacheService
{
    private readonly Dictionary<string, object> _cache = new();

    public Task<T?> GetAsync<T>(string key)
    {
        if (_cache.TryGetValue(key, out var value))
        {
            return Task.FromResult((T?)value);
        }
        return Task.FromResult(default(T));
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        _cache[key] = value!;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }

    public Task RemoveByPatternAsync(string pattern)
    {
        var keysToRemove = _cache.Keys.Where(k => k.Contains(pattern.Replace("*", ""))).ToList();
        foreach (var key in keysToRemove)
        {
            _cache.Remove(key);
        }
        return Task.CompletedTask;
    }
}