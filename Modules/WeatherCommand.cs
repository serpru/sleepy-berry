using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SleepyBerry.Services;

namespace SleepyBerry.Modules
{

    // Base of weather command
    [Group("wth")]
    public class WeatherCommand : ModuleBase
    {
        private readonly IServiceProvider _services;

        public WeatherCommand(IServiceProvider services)
        {
            _services = services;
        }

        [Command]
        [Summary("Displays the weather of city provided")]
        public async Task WthCommand([Remainder]string city = null)
        {
            // initialize empty string builder for reply
            var sb = new StringBuilder();

            if (city == null)
            {
                // if no city is provided (args are null), reply with the below text
                sb.AppendLine("I need a city to check the weather, please type in a city after the command");
                await ReplyAsync(sb.ToString());
            }
            else
            {

                // let's use an embed for this one!
                var embed = new EmbedBuilder();
                embed.WithColor(new Color(0, 100, 255));
                embed.Title = "Current weather:";

                // Get weather service
                var ws = _services.GetService<WeatherService>();

                // Grab data from weather service
                var rawWeather = await ws.City(city) as OpenWeatherResponse;

                // build out the reply
                //sb.AppendLine($"Current weather:");
                sb.AppendLine(rawWeather.Name);

                if (!String.IsNullOrEmpty(rawWeather.Main.Temp))
                {
                    sb.AppendLine($"Location: {rawWeather.Sys.Country}");
                    sb.AppendLine($"Temperature: {rawWeather.Main.Temp}\u00B0C");
                    sb.AppendLine($"Pressure: {rawWeather.Main.Pressure}hPa");
                    sb.AppendLine($"Weather decription: {rawWeather.Weather.First().Main}");
                    sb.AppendLine();
                    sb.AppendLine("*Weather provided by OpenWeather*");
                }

                embed.Description = sb.ToString();

                // this will reply with the embed
                await ReplyAsync(null, false, embed.Build());
            }
        }
    }

}