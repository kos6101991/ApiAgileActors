using ApiAggileActors.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ApiAggileActors.Services
{
    public class NewsApiService : INewsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly NewsApiSettings _settings;
        private readonly IMemoryCache _cache;
        public NewsApiService(HttpClient httpClient, IOptions<NewsApiSettings> settings, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _cache = cache;
        }

        public async Task<string> GetNewsAsync(string query)
        {
            var cacheKey = string.Format(_settings.CacheKey, query);

            // Check if we have cached data for the specific query
            if (_cache.TryGetValue(cacheKey, out string? cachedNews))
            {
                return cachedNews ?? string.Empty;
            }

            // if the cache is empty call the API and put them in the cache with the above key
            var url = $"{_settings.BaseUrl}everything?q={query}&apiKey={_settings.ApiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var newsData = await response.Content.ReadAsStringAsync();

            // store data for cache expiry minutes
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.CacheExpiry));

            _cache.Set(cacheKey, newsData, cacheOptions);

            return newsData;
        }
    }
}
