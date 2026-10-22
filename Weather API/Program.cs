using Weather_API.Services;
using Weather_API.Services.Interfaces;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

var apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY");

if (string.IsNullOrWhiteSpace(apiKey))
{
    throw new Exception("API key not found");
}

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "http://localhost:3000",
                "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API V1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCors("FrontendPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();