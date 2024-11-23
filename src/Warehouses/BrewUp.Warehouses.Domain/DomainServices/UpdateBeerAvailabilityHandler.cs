using BrewUp.Warehouses.Domain.Entities;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace BrewUp.Warehouses.Domain.DomainServices;

public sealed class UpdateBeerAvailabilityHandler<UpdateAvailabilityDueToProductionOrder>(IRepository repository,
    ILoggerFactory loggerFactory)
    : BrewUpCommandHandlerBase<SharedKernel.Commands.UpdateAvailabilityDueToProductionOrder>(repository, loggerFactory)
{
    public override async Task HandleAsync(SharedKernel.Commands.UpdateAvailabilityDueToProductionOrder command, CancellationToken cancellationToken)
    {
        var aggregate = await Repository.GetByIdAsync<Availability>(command.BeerId, cancellationToken);
        aggregate!.UpdateAvailability(command.Quantity, command.MessageId);

        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}