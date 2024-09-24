using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class WeatherService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public WeatherService(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
    }

    public async Task<WeatherData> GetWeather(string city)
    {
        
       var response = await _httpClient.GetAsync($"http://api.weatherapi.com/v1/current.json?key={_apiKey}&q={city}");

        if (!response.IsSuccessStatusCode)
        {
            throw new WeatherServiceException("Invalid city or API error.");
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<WeatherData>(json);
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


public class WeatherData
{
    public Location Location { get; set; }
    public Current Current { get; set; }
}

public class Location
{
    public string Name { get; set; }
}

public class Current
{
    public float temp_c { get; set; }
    public Condition Condition { get; set; }
    public int IsDay { get; set; }
    public int localtime { get; set; }
}

public class Condition
{
    public string Text { get; set; }
}

public class WeatherServiceException : Exception
{
    public WeatherServiceException(string message) : base(message) { }
}