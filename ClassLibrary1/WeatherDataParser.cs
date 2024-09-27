using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClassLibrary1
{
    public class WeatherDataParser : IWeatherDataParser
    {
        public WeatherData Parse(string json)
        {
            return JsonConvert.DeserializeObject<WeatherData>(json);
        }
    }
}
