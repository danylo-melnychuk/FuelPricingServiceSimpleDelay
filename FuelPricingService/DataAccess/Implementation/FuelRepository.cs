using FuelPricingService.Configuration.Models;
using FuelPricingService.DataAccess.Abstraction;
using FuelPricingService.Domain.Entities;
using Microsoft.Extensions.Options;

namespace FuelPricingService.DataAccess.Implementation;

public class FuelRepository : BaseRepository, IFuelRepository
{
	private readonly SystemSettings _systemSettings;

	public FuelRepository(
		IDataConnection dataConnection,
		ILogger<FuelRepository> logger,
		IOptions<SystemSettings> systemSettings) : base(dataConnection, logger)
	{
		_systemSettings = systemSettings.Value;
	}

	public Task SaveFuelPrice(
		IEnumerable<FuelPrice> prices) =>
		ExecuteAsync(
			StoredProcedureNames.InsertFuelPrice, 
			new
			{
				Prices = TableTypeHelper.CreateFuelPriceTable(prices),
				_systemSettings.RetentionPeriodDays
			});
}