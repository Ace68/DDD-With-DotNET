﻿using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;

namespace BrewUp.Warehouses.ReadModel.Services;

public interface IAvailabilityService
{
	Task CreateAvailabilityAsync(BeerId beerId, BeerName beerName, Quantity quantity, CancellationToken cancellationToken = default);
	Task UpdateAvailabilityAsync(BeerId beerId, BeerName beerName, Quantity quantity, CancellationToken cancellationToken = default);
}