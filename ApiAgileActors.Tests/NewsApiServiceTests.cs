using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;

public class NewsApiServiceTests
{
    [Fact]
    public async Task GetNewsAsync_ReturnsData_WhenApiCallIsSuccessful()
    {
        // Arrange
        var newsServiceMock = new Mock<INewsApiService>();
        newsServiceMock.Setup(s => s.GetNewsAsync(It.IsAny<string>()))
            .ReturnsAsync("{\"news\":\"latest\"}");

        var service = newsServiceMock.Object;

        // Act
        var result = await service.GetNewsAsync("test");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("latest", result);
    }

    [Fact]
    public async Task GetNewsAsync_ThrowsException_WhenApiCallFails()
    {
        // Arrange
        var newsServiceMock = new Mock<INewsApiService>();
        newsServiceMock.Setup(s => s.GetNewsAsync(It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException("API call failed"));

        var service = newsServiceMock.Object;

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => service.GetNewsAsync("test"));
    }
}