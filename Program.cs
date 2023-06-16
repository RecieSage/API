using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

string sqlConnectionString = configuration.GetConnectionString("SQLServerConnection");

// Set SQL connection string as env variable
Environment.SetEnvironmentVariable("SQL_CONNECTION_STRING", configuration.GetConnectionString("SQLServerConnection"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
