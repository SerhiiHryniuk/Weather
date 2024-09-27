using ClassLibrary1;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

[TestFixture]
public class WeatherServiceTests
{
    private const string ApiKey = "2e276411b20744f28f6185225242409";
    private WeatherService _service;

    [Test]
    public async Task GetWeather_ShouldReturnWeatherData_WhenValidCityProvided()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClient>();
        var weatherDataParserMock = new Mock<IWeatherDataParser>();
        _service = new WeatherService(ApiKey, httpClientMock.Object, weatherDataParserMock.Object);

        string city = "London";
        var weatherData = new WeatherData
        {
            Location = new Location { Name = "London" },
            Current = new Current { temp_c = 20 }
        };

        var json = JsonConvert.SerializeObject(weatherData);
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        };

        httpClientMock.Setup(client => client.GetAsync(It.IsAny<string>()))
                       .ReturnsAsync(httpResponseMessage);
        weatherDataParserMock.Setup(parser => parser.Parse(json)).Returns(weatherData);

        // Act
        var result = await _service.GetWeather(city);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Location.Name, Is.EqualTo("London"));
    }

    [Test]
    public void GetWeather_ShouldThrowException_WhenInvalidCityProvided()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClient>();
        var weatherDataParserMock = new Mock<IWeatherDataParser>();
        _service = new WeatherService(ApiKey, httpClientMock.Object, weatherDataParserMock.Object);

        string city = "InvalidCity";
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

        httpClientMock.Setup(client => client.GetAsync(It.IsAny<string>()))
                       .ReturnsAsync(httpResponseMessage);

        // Act & Assert
        var ex = Assert.ThrowsAsync<WeatherServiceException>(async () => await _service.GetWeather(city));
        Assert.That(ex.Message, Is.EqualTo("Invalid city or API error."));
    }

    [Test]
    public async Task GetWeather_ShouldReturnTemperatureInCelsius_WhenValidCityProvided()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClient>();
        var weatherDataParserMock = new Mock<IWeatherDataParser>();
        _service = new WeatherService(ApiKey, httpClientMock.Object, weatherDataParserMock.Object);

        string city = "London";
        var weatherData = new WeatherData
        {
            Location = new Location { Name = "London" },
            Current = new Current { temp_c = 20 }
        };

        var json = JsonConvert.SerializeObject(weatherData);
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        };

        httpClientMock.Setup(client => client.GetAsync(It.IsAny<string>()))
                       .ReturnsAsync(httpResponseMessage);
        weatherDataParserMock.Setup(parser => parser.Parse(json)).Returns(weatherData);

        // Act
        var result = await _service.GetWeather(city);

        // Assert
        Assert.That(result.Current.temp_c, Is.InRange(-100, 100));
    }

    [Test]
    public async Task GetTemperatureMessage_ShouldReturnHotMessage_WhenTemperatureAbove30()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClient>();
        var weatherDataParserMock = new Mock<IWeatherDataParser>();
        _service = new WeatherService(ApiKey, httpClientMock.Object, weatherDataParserMock.Object);

        string city = "Dubai";
        var weatherData = new WeatherData
        {
            Current = new Current { temp_c = 35 }
        };

        var json = JsonConvert.SerializeObject(weatherData);
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        };

        httpClientMock.Setup(client => client.GetAsync(It.IsAny<string>()))
                       .ReturnsAsync(httpResponseMessage);
        weatherDataParserMock.Setup(parser => parser.Parse(json)).Returns(weatherData);

        // Act
        var result = await _service.GetWeather(city);
        var message = _service.GetTemperatureMessage(result.Current.temp_c);

        // Assert
        Assert.That(message, Is.EqualTo("It's a hot day!"));
    }

    [Test]
    public async Task GetTemperatureMessage_ShouldReturnModerateMessage_WhenTemperatureBelow30Above13()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClient>();
        var weatherDataParserMock = new Mock<IWeatherDataParser>();
        _service = new WeatherService(ApiKey, httpClientMock.Object, weatherDataParserMock.Object);

        string city = "London";
        var weatherData = new WeatherData
        {
            Current = new Current { temp_c = 20 }
        };

        var json = JsonConvert.SerializeObject(weatherData);
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        };

        httpClientMock.Setup(client => client.GetAsync(It.IsAny<string>()))
                       .ReturnsAsync(httpResponseMessage);
        weatherDataParserMock.Setup(parser => parser.Parse(json)).Returns(weatherData);

        // Act
        var result = await _service.GetWeather(city);
        var message = _service.GetTemperatureMessage(result.Current.temp_c);

        // Assert
        Assert.That(message, Is.EqualTo("The weather is moderate."));
    }

    [Test]
    public async Task GetTemperatureMessage_ShouldReturnColdMessage_WhenTemperatureBelow13()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClient>();
        var weatherDataParserMock = new Mock<IWeatherDataParser>();
        _service = new WeatherService(ApiKey, httpClientMock.Object, weatherDataParserMock.Object);

        string city = "Juneau";
        var weatherData = new WeatherData
        {
            Current = new Current { temp_c = 10 }
        };

        var json = JsonConvert.SerializeObject(weatherData);
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        };

        httpClientMock.Setup(client => client.GetAsync(It.IsAny<string>()))
                       .ReturnsAsync(httpResponseMessage);
        weatherDataParserMock.Setup(parser => parser.Parse(json)).Returns(weatherData);

        // Act
        var result = await _service.GetWeather(city);
        var message = _service.GetTemperatureMessage(result.Current.temp_c);

        // Assert
        Assert.That(message, Is.EqualTo("The weather is cold."));
    }
}