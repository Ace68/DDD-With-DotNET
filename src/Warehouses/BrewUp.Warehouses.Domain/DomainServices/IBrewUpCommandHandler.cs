using Muflone.Messages.Commands;

namespace BrewUp.Warehouses.Domain.DomainServices;

public interface IBrewUpCommandHandler<in T> where T : ICommand
{
    Task HandleAsync(T command, CancellationToken cancellationToken);
}