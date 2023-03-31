using FuelPricingService;
using FuelPricingService.Configuration;
using FuelPricingService.Configuration.Models;
using FuelPricingService.DataAccess;
using FuelPricingService.DataAccess.Abstraction;
using FuelPricingService.DataAccess.Implementation;
using FuelPricingService.Services.Abstraction;
using FuelPricingService.Services.Implementation;
using Serilog;

LogConfiguration.ConfigureLogging();

var host = Host.CreateDefaultBuilder(args)
	.ConfigureAppConfiguration((hostContext, configuration) =>
	{
		configuration.SetBasePath(hostContext.HostingEnvironment.ContentRootPath);
		configuration.AddJsonFile("appsettings.json", true, true);
#if DEBUG
		configuration.AddJsonFile("appsettings.Development.json", true, true);
#else
		configuration.AddJsonFile(
			$"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
			true,
			true);
#endif
		configuration.AddEnvironmentVariables();
	})
	.ConfigureServices((hostContext, services) =>
	{
		services.AddTransient<IDataConnection, DataConnection>();
		services.AddTransient<IFuelRepository, FuelRepository>();
		services.AddTransient<IFuelPriceService, FuelPriceService>();
		services.Configure<SystemSettings>(hostContext.Configuration.GetSection("SystemSettings"));
		services.AddHttpClient("FuelPriceClient")
			.AddPolicyHandler(HttpClientConfiguration.RetryPolicy);
		
		services.AddHostedService<FuelPriceWorker>();
	})
	.UseSerilog()
	.Build();

DbUpConfiguration.MigrateDatabase();

await host.RunAsync();