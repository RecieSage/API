namespace API
{
    /// <summary>
    /// Class representing a WeatherForecast object
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// Date of the WeatherForecast
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Temperature in Celsius
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// Temperature in Fahrenheit
        /// </summary>
        public int TemperatureF => 32 + (int)(this.TemperatureC / 0.5556);

        /// <summary>
        /// Summary of the WeatherForecast
        /// </summary>
        public string? Summary { get; set; }
    }
}