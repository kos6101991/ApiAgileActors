using System.Threading.Tasks;

public interface ITwitterService
{
    Task<string> GetTweetsAsync(string query);
}