using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SleepyBerry.Services
{
    public class WeatherService : Controller
    {
        private readonly IConfiguration _config;
        private readonly IServiceProvider _services;
        public WeatherService(IServiceProvider services)
        {
            _config = services.GetRequiredService<IConfiguration>();
            _services = services;
        }

        public async Task<OpenWeatherResponse> City(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid={_config["WeatherKey"]}&units=metric");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);
                    return rawWeather;
                }
                catch (HttpRequestException httpRequestException)
                {
                    return new OpenWeatherResponse("Error getting weather from OpenWeather", httpRequestException.Message);
                    //BadRequest($"Error getting weather from OpenWeather: {}");
                }
            }
        }
    }

    public class OpenWeatherResponse
    {
        public OpenWeatherResponse(string title, string message)
        {
            Name = title + "\n" + message;
            Weather = new List<WeatherDescription>() { new WeatherDescription() };
            Sys = new Sys();
            Main = new Main();

        }
        public string Name { get; set; }

        public Sys Sys { get; set; }

        public IEnumerable<WeatherDescription> Weather { get; set; }

        public Main Main { get; set; }
    }

    public class WeatherDescription
    {
        public WeatherDescription(){
            Main = "";
            Description = "";
        }
        public string Main { get; set; }
        public string Description { get; set; }
    }

    public class Main
    {
        public Main()
        {
            Temp = "";
            Pressure = "";
        }
        public string Temp { get; set; }
        public string Pressure { get; set; }
    }

    public class Sys
    {
        public Sys()
        {
            Country = "";
        }
        public string Country { get; set; }
    }
}