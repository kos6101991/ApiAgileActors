using ApiAggileActors.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ApiAggileActors.Services
{
    public class OpenWeatherMapService : IOpenWeatherMapService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherMapSettings _settings;
        private readonly IMemoryCache _cache;
        public OpenWeatherMapService(HttpClient httpClient, IOptions<OpenWeatherMapSettings> settings, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _cache = cache;
        }

        public async Task<string> GetWeatherAsync(string city)
        {
            var cacheKey = string.Format(_settings.CacheKey, city);

            // Check if we have cached data for the specific city
            if (_cache.TryGetValue(cacheKey, out string? cachedWeather))
            {
                return cachedWeather ?? string.Empty;
            }

            // if the cache is empty call the API and put them in the cache with the above key
            var url = $"{_settings.BaseUrl}weather?q={city}&appid={_settings.ApiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var weatherData = await response.Content.ReadAsStringAsync() ?? string.Empty;

            // store data for cache expiry minutes
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.CacheExpiry));

            _cache.Set(cacheKey, weatherData, cacheOptions);

            return weatherData;
        }
    }
}
