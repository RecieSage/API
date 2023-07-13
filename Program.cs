using API.Backend;
using System.Configuration;
using System.Reflection;

namespace API
{
    /// <summary>
    /// The main class of the API
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method of the API
        /// </summary>
        /// <param name="args">The arguments of the API</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Check if appsettings.json exists
            if (!File.Exists("appsettings.json"))
            {
                Console.WriteLine("appsettings.json not found!");
                Environment.Exit(1);
            }

            // Read appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            // Set SQL connection string as env variable
            Environment.SetEnvironmentVariable("SQL_CONNECTION_STRING", configuration.GetConnectionString("SQLServerConnection"));

            DatabaseUpdater databaseUpdater = new DatabaseUpdater();
            if (databaseUpdater.IsUpdateAvailable())
            {
                databaseUpdater.Update();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}