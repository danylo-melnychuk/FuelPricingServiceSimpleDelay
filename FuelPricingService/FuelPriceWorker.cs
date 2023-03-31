using FuelPricingService.Configuration.Models;
using FuelPricingService.Services.Abstraction;
using Microsoft.Extensions.Options;

namespace FuelPricingService;

public class FuelPriceWorker : BackgroundService
{
	private readonly ILogger<FuelPriceWorker> _logger;
	private readonly IFuelPriceService _fuelPriceService;
	private readonly SystemSettings _systemSettings;

	public FuelPriceWorker(
		ILogger<FuelPriceWorker> logger,
		IFuelPriceService fuelPriceService,
		IOptions<SystemSettings> systemSettings)
	{
		_logger = logger;
		_fuelPriceService = fuelPriceService;
		_systemSettings = systemSettings.Value;
	}

	protected override async Task ExecuteAsync(
		CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
			
			await _fuelPriceService.SavePrices(stoppingToken);

			await Task.Delay(
				TimeSpan.FromMinutes(_systemSettings.DelayMinutes),
				stoppingToken);
		}
	}
}