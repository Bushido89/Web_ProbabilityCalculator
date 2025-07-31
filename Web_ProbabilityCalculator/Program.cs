using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var loggingOutputTemplate = "{Timestamp:HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";
Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Debug()
	.WriteTo.Console(
		outputTemplate: loggingOutputTemplate)
	.WriteTo.File(
		$@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Logs\WebService_DeviceMessageCommsLayer_.log",
		rollingInterval: RollingInterval.Day,
		outputTemplate: loggingOutputTemplate,
		flushToDiskInterval: TimeSpan.FromSeconds(1),
		buffered: false
	).CreateLogger();
Log.Debug("Application initialization!");


builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowReactApp",
		policy => policy.WithOrigins("http://localhost:5173")
			.AllowAnyHeader()
			.AllowAnyMethod());
});




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddSingleton

var app = builder.Build();

app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
