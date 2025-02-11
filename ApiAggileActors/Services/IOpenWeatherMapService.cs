using System.Threading.Tasks;

public interface IOpenWeatherMapService
{
    Task<string> GetWeatherAsync(string city);
}