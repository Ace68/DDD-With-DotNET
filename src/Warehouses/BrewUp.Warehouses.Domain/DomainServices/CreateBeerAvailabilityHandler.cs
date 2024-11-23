using BrewUp.Warehouses.Domain.Entities;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace BrewUp.Warehouses.Domain.DomainServices;

public sealed class CreateBeerAvailabilityHandler<CreateBeerAvailability>(IRepository repository,
    ILoggerFactory loggerFactory)
    : BrewUpCommandHandlerBase<SharedKernel.Commands.CreateBeerAvailability>(repository, loggerFactory)
{
    public override async Task HandleAsync(SharedKernel.Commands.CreateBeerAvailability command, CancellationToken cancellationToken)
    {
        var aggregate = Availability.CreateAvailability(command.BeerId, command.BeerName, command.Quantity, command.MessageId);
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}