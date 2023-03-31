namespace FuelPricingService.Services.Abstraction;

public interface IFuelPriceService
{
	Task SavePrices(CancellationToken cancellationToken);
}