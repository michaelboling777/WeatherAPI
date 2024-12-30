using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

class Program
{
    private static readonly string apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY") ?? "f263694865140e6a409e02d24f4fae49";
    private static readonly string baseUrl = "https://api.openweathermap.org/data/2.5/weather";
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Error: API key not found. Please set the OPENWEATHER_API_KEY environment variable.");
            return;
        }

        while (true)
        {
            Console.WriteLine("\nEnter a city name and region to get the current weather (e.g., 'Springfield, Missouri') or 'exit' to quit:");
            string location = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(location))
            {
                Console.WriteLine("Location cannot be empty. Please try again.");
                continue;
            }

            if (location.ToLower() == "exit")
            {
                break;
            }

            await GetWeatherAsync(location);
        }
    }

    private static async Task GetWeatherAsync(string location)
    {
        try
        {
            string url = $"{baseUrl}?q={location}&appid={apiKey}&units=imperial";
            var response = await client.GetFromJsonAsync<WeatherResponse>(url);

            if (response != null)
            {
                Console.WriteLine($"Current weather in {location}: {response.Main.Temp}°F, {response.Weather[0].Description}");
            }
            else
            {
                Console.WriteLine("Error: Unable to get weather data.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

public class WeatherResponse
{
    public Main Main { get; set; }
    public Weather[] Weather { get; set; }
}

public class Main
{
    public float Temp { get; set; }
}

public class Weather
{
    public string Description { get; set; }
}