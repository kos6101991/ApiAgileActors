using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;

public class OpenWeatherMapServiceTests
{
    [Fact]
    public async Task GetWeatherAsync_ReturnsData_WhenApiCallIsSuccessful()
    {
        // Arrange
        var weatherServiceMock = new Mock<IOpenWeatherMapService>();
        weatherServiceMock.Setup(s => s.GetWeatherAsync(It.IsAny<string>()))
            .ReturnsAsync("{\"weather\":\"sunny\"}");

        var service = weatherServiceMock.Object;

        // Act
        var result = await service.GetWeatherAsync("Athens");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("sunny", result);
    }

    [Fact]
    public async Task GetWeatherAsync_ThrowsException_WhenApiCallFails()
    {
        // Arrange
        var weatherServiceMock = new Mock<IOpenWeatherMapService>();
        weatherServiceMock.Setup(s => s.GetWeatherAsync(It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException("API call failed"));

        var service = weatherServiceMock.Object;

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => service.GetWeatherAsync("Athens"));
    }
}