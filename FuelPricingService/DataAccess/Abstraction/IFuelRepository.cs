using FuelPricingService.Domain.Entities;

namespace FuelPricingService.DataAccess.Abstraction;

public interface IFuelRepository
{
	Task SaveFuelPrice(IEnumerable<FuelPrice> prices);
}