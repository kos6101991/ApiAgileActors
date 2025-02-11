using ApiAggileActors.Services;
using ApiAggileActors.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Προσθήκη των services με βάση τα interfaces
builder.Services.AddHttpClient<IOpenWeatherMapService, OpenWeatherMapService>();
//builder.Services.AddHttpClient<INewsApiService, NewsApiService>();
//builder.Services.AddHttpClient<ITwitterService, TwitterService>();


builder.Services.Configure<OpenWeatherMapSettings>(options =>
{
    options.ApiKey = "openweathermap_api_key";
    options.BaseUrl = "https://api.openweathermap.org/data/2.5/";
    options.CacheKey = "Weather_{0}";
    options.CacheExpiry = 10;
});

builder.Services.Configure<NewsApiSettings>(options =>
{
    options.ApiKey = "newsapi_api_key";
    options.BaseUrl = "https://newsapi.org/v2/";
    options.CacheKey = "News_{0}";
    options.CacheExpiry = 10;
});

builder.Services.Configure<TwitterSettings>(options =>
{
    options.ApiKey = "twitter_api_key";
    options.ApiSecretKey = "twitter_api_secret_key";
    options.BaseUrl = "https://api.twitter.com/2/";
    options.CacheKey = "Tweets_{0}";
    options.CacheExpiry = 10;
});

builder.Services.AddMemoryCache();

builder.Services.AddControllers();

builder.Services.AddHttpClient<IOpenWeatherMapService, OpenWeatherMapService>();
builder.Services.AddHttpClient<INewsApiService, NewsApiService>();
builder.Services.AddHttpClient<ITwitterService, TwitterService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
