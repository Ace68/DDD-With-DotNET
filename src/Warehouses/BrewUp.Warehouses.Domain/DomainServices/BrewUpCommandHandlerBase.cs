using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace BrewUp.Warehouses.Domain.DomainServices;

public abstract class BrewUpCommandHandlerBase<T> : IBrewUpCommandHandler<T> where T : ICommand
{
    protected readonly IRepository Repository;
    protected readonly ILogger Logger;

    protected BrewUpCommandHandlerBase(IRepository repository,
        ILoggerFactory loggerFactory)
    {
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        Logger = loggerFactory.CreateLogger(GetType()) ?? throw new ArgumentNullException(nameof(loggerFactory));
    }
    public abstract Task HandleAsync(T command, CancellationToken cancellationToken);
}