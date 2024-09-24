using NUnit.Framework;
using System.Threading.Tasks;

[TestFixture]
public class WeatherServiceTests
{
    private const string ApiKey = "2e276411b20744f28f6185225242409";
    private WeatherService _service;

    [SetUp]
    public void Setup()
    {
        _service = new WeatherService(ApiKey);
    }

    [Test]
    public async Task GetWeather_ShouldReturnWeatherData_WhenValidCityProvided()
    {
        // Arrange
        string city = "London";

        // Act
        var result = await _service.GetWeather(city);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("London", result.Location.Name);
    }

    [Test]
    public void GetWeather_ShouldThrowException_WhenInvalidCityProvided()
    {
        // Arrange
        string city = "InvalidCity";

        // Act & Assert
        var ex = Assert.ThrowsAsync<WeatherServiceException>(async () => await _service.GetWeather(city));
        Assert.AreEqual("Invalid city or API error.", ex.Message);
    }
    

    [Test]
    public async Task GetWeather_ShouldReturnTemperatureInCelsius_WhenValidCityProvided()
    {
        // Arrange
        string city = "London";

        // Act
        var result = await _service.GetWeather(city);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Current.temp_c >= -100 && result.Current.temp_c <= 100,
            "Temperature should be a reasonable value in Celsius.");
    }

    [Test]
    public async Task GetTemperatureMessage_ShouldReturnHotMessage_WhenTemperatureAbove30()
    {
        // Arrange
        string city = "Dubai";

        // Act
        var result = await _service.GetWeather(city);
        var message = _service.GetTemperatureMessage(result.Current.temp_c);

        // Assert
        Assert.AreEqual("It's a hot day!", message);
    }

    [Test]
    public async Task GetTemperatureMessage_ShouldReturnHotMessage_WhenTemperatureBelow30Above13()
    {
        // Arrange
        string city = "London";

        // Act
        var result = await _service.GetWeather(city);
        var message = _service.GetTemperatureMessage(result.Current.temp_c);

        // Assert
        Assert.AreEqual("The weather is moderate.", message);
    }

    [Test]
    public async Task GetTemperatureMessage_ShouldReturnHotMessage_WhenTemperatureBelow13()
    {
        // Arrange
        string city = "Juneau";

        // Act
        var result = await _service.GetWeather(city);
        var message = _service.GetTemperatureMessage(result.Current.temp_c);

        // Assert
        Assert.AreEqual("The weather is cold.", message);
    }
}