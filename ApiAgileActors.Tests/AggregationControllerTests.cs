using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ApiAggileActors.Services;
using ApiAggileActors.Settings;
using ApiAggileActors.Controllers;

public class AggregationControllerTests
{
    [Fact]
    public async Task AggregateData_ReturnsOkResult_WhenAllServicesAreSuccessful()
    {
        // Arrange
        var weatherServiceMock = new Mock<IOpenWeatherMapService>();
        weatherServiceMock.Setup(s => s.GetWeatherAsync(It.IsAny<string>()))
            .ReturnsAsync("{\"weather\":\"sunny\"}");

        var newsServiceMock = new Mock<INewsApiService>();
        newsServiceMock.Setup(s => s.GetNewsAsync(It.IsAny<string>()))
            .ReturnsAsync("{\"news\":\"latest\"}");

        var twitterServiceMock = new Mock<ITwitterService>();
        twitterServiceMock.Setup(s => s.GetTweetsAsync(It.IsAny<string>()))
            .ReturnsAsync("{\"tweets\":\"recent\"}");

        var controller = new AggregationController(
            weatherServiceMock.Object,
            newsServiceMock.Object,
            twitterServiceMock.Object
        );

        // Act
        var result = await controller.AggregateData("Athens", "test");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task AggregateData_ReturnsError_WhenAnyServiceFails()
    {
        // Arrange
        var weatherServiceMock = new Mock<IOpenWeatherMapService>();
        weatherServiceMock.Setup(s => s.GetWeatherAsync(It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException("API call failed"));

        var newsServiceMock = new Mock<INewsApiService>();
        newsServiceMock.Setup(s => s.GetNewsAsync(It.IsAny<string>()))
            .ReturnsAsync("{\"news\":\"latest\"}");

        var twitterServiceMock = new Mock<ITwitterService>();
        twitterServiceMock.Setup(s => s.GetTweetsAsync(It.IsAny<string>()))
            .ReturnsAsync("{\"tweets\":\"recent\"}");

        var controller = new AggregationController(
            weatherServiceMock.Object,
            newsServiceMock.Object,
            twitterServiceMock.Object
        );

        // Act
        var result = await controller.AggregateData("Athens", "test");

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}