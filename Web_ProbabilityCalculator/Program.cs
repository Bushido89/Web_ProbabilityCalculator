using Serilog;
using System.Reflection;
using Web_ProbabilityCalculator;
using Web_ProbabilityCalculator.Services;


var builder = WebApplication.CreateBuilder(args);

// Setup Serilog
var loggingOutputTemplate = "{Timestamp:HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";
var logPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Logs", "WebService_DeviceMessageCommsLayer_.log");

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Debug()
	.WriteTo.Console(outputTemplate: loggingOutputTemplate)
	.WriteTo.File(
		logPath,
		rollingInterval: RollingInterval.Day,
		outputTemplate: loggingOutputTemplate,
		flushToDiskInterval: TimeSpan.FromSeconds(1),
		buffered: false)
	.CreateLogger();

Log.Debug("Application initialization!");

// CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowReactApp", policy =>
		policy.WithOrigins("https://localhost:7115") // Replace with your actual origin
			  .AllowAnyHeader()
			  .AllowAnyMethod());
});

// Add services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Load full config object
var conf = builder.Configuration.Get<AppSettings>();
builder.Services.AddSingleton<ICalculationService>(new CalculationService(conf!));

var app = builder.Build();

app.MapControllerRoute(
	name: "default",
	pattern: "api/{controller}/{action=Index}");

app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();