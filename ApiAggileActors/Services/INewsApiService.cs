using System.Threading.Tasks;

public interface INewsApiService
{
    Task<string> GetNewsAsync(string query);
}