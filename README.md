
API Agiles Actors Documentation


#Overview
This project is an ASP.NET Core Web API that combines data from multiple external APIs (OpenWeatherMap, News API, and Twitter API) and provides a single endpoint for retrieving the data. The API is designed to be extensible and supports easy addition of new APIs in the future.

##################################################################################################

#Requirements
.NET 7 SDK
Visual Studio 2022 
API keys for the external APIs (OpenWeatherMap, News API, Twitter API)

##################################################################################################

#Installation
You can download the repository:
git clone https://github.com/kos6101991/ApiAgileActors.git


Install the necessary packages:
dotnet restore

Set up the API keys:  Open Program.cs and update the settings for the APIs:

builder.Services.Configure<OpenWeatherMapSettings>(options =>
{
    options.ApiKey = "your_openweathermap_api_key";
    options.BaseUrl = "https://api.openweathermap.org/data/2.5/";
});

builder.Services.Configure<NewsApiSettings>(options =>
{
    options.ApiKey = "your_newsapi_api_key";
    options.BaseUrl = "https://newsapi.org/v2/";
});

builder.Services.Configure<TwitterSettings>(options =>
{
    options.ApiKey = "your_twitter_api_key";
    options.ApiSecretKey = "your_twitter_api_secret_key";
    options.BaseUrl = "https://api.twitter.com/2/";
});

##################################################################################################

#Run the project:
cd ApiAggileActors
dotnet run
##################################################################################################



#Endpoints
GET /api/aggregation/aggregate
This endpoint combines data from the external APIs and returns it in JSON format.

Parameters
city (string): The city for which you want weather data (e.g., "Athens").
query (string): The query for news and tweets (e.g., "technology").

Example Request
GET /api/aggregation/aggregate?city=Athens&query=technology

Example Response
json
Copy
{
  "weather": {
    "main": "Clear",
    "description": "clear sky",
    "temp": 20.5
  },
  "news": [
    {
      "title": "New Tech Innovation",
      "description": "A new tech innovation has been announced...",
      "url": "https://example.com/news/1"
    }
  ],
  "tweets": [
    {
      "text": "Check out this new tech gadget!",
      "user": "techlover123"
    }
  ]
}
##################################################################################################
##################################################################################################
##################################################################################################



###################################  Architecture
#Services and Interfaces

OpenWeatherMapService, IOpenWeatherMapService: Retrieves weather data from the OpenWeatherMap API.
NewsApiService, INewsApiService: Retrieves news from the News API.
TwitterService, ITwitterService: Retrieves tweets from the Twitter API.

Controller
AggregationController: Combines data from the services and returns it through an endpoint.

Settings
OpenWeatherMapSettings, NewsApiSettings, TwitterSettings: Contains the settings for the APIs (API keys, base URLs).


##################################################################################################



#Unit Tests
The unit tests check the functionality of the services and the controller. To run the tests:

Go to the test project folder: cd ApiAggregator.Tests
Run the tests: dotnet test