using ApiAggileActors.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace ApiAggileActors.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly HttpClient _httpClient;
        private readonly TwitterSettings _settings;
        private readonly IMemoryCache _cache;
        public TwitterService(HttpClient httpClient, IOptions<TwitterSettings> settings, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _cache = cache;
        }

        public async Task<string> GetTweetsAsync(string query)
        {
            var cacheKey = string.Format(_settings.CacheKey, query);

            if (_cache.TryGetValue(cacheKey, out string? cachedTweets))
            {
                return cachedTweets ?? string.Empty;
            }

            var url = $"{_settings.BaseUrl}tweets/search/recent?query={query}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var tweetsData = await response.Content.ReadAsStringAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.CacheExpiry));

            _cache.Set(cacheKey, tweetsData, cacheOptions);

            return tweetsData;
        }
    }
}
