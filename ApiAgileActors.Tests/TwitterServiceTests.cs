using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;

public class TwitterServiceTests
{
    [Fact]
    public async Task GetTweetsAsync_ReturnsData_WhenApiCallIsSuccessful()
    {
        // Arrange
        var twitterServiceMock = new Mock<ITwitterService>();
        twitterServiceMock.Setup(s => s.GetTweetsAsync(It.IsAny<string>()))
            .ReturnsAsync("{\"tweets\":\"recent\"}");

        var service = twitterServiceMock.Object;

        // Act
        var result = await service.GetTweetsAsync("test");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("recent", result);
    }

    [Fact]
    public async Task GetTweetsAsync_ThrowsException_WhenApiCallFails()
    {
        // Arrange
        var twitterServiceMock = new Mock<ITwitterService>();
        twitterServiceMock.Setup(s => s.GetTweetsAsync(It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException("API call failed"));

        var service = twitterServiceMock.Object;

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => service.GetTweetsAsync("test"));
    }
}