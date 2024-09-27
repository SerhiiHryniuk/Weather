using System.Net.Http;
using System.Threading.Tasks;
using ClassLibrary1;
using Newtonsoft.Json;

public class WeatherService
{
    private readonly string _apiKey;
    private readonly IHttpClient _httpClient;
    private readonly IWeatherDataParser _weatherDataParser;

    public WeatherService(string apiKey, IHttpClient httpClient, IWeatherDataParser weatherDataParser)
    {
        _apiKey = apiKey;
        _httpClient = httpClient;
        _weatherDataParser = weatherDataParser;
    }


    public async Task<WeatherData> GetWeather(string city)
    {
        var response = await _httpClient.GetAsync($"http://api.weatherapi.com/v1/current.json?key={_apiKey}&q={city}");

        if (!response.IsSuccessStatusCode)
        {
            throw new WeatherServiceException("Invalid city or API error.");
        }

        var json = await response.Content.ReadAsStringAsync();
        return _weatherDataParser.Parse(json);
    }

    public string GetTemperatureMessage(float tempC)
    {
        if (tempC > 30)
        {
            return "It's a hot day!";
        }
        else if (tempC <= 30 && tempC >= 13)
        {
            return "The weather is moderate.";
        }
        else
        {
            return "The weather is cold.";
        }
    }
}

public class WeatherServiceException : Exception
{
    public WeatherServiceException(string message) : base(message) { }
}