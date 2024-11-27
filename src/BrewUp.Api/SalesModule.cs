using BrewUp.Sales.Facade;
using BrewUp.Sales.Facade.Validators;
using BrewUp.Sales.ReadModel.Dtos;
using BrewUp.Sales.ReadModel.Queries;
using BrewUp.Sales.ReadModel.Services;
using BrewUp.Shared.ReadModel;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace BrewUp.Api;

public static class SalesModule
{
    public static void RegisterSalesModule(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<SalesOrderContractValidator>();
        services.AddSingleton<ValidationHandler>();

        services.AddScoped<ISalesFacade, SalesFacade>();
        services.AddScoped<ISalesOrderService, SalesOrderService>();
        services.AddScoped<IAvailabilityService, AvailabilityService>();
        services.AddScoped<IQueries<SalesOrder>, SalesOrderQueries>();
        services.AddScoped<IQueries<Availability>, AvailabilityQueries>();
    }
    
    public static void ConfigureSalesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/v1/sales/")
            .WithTags("Sales");

        group.MapGet("/", HandleGetOrders)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("GetSalesOrders");
    }
    
    private static async Task<IResult> HandleGetOrders(
        ISalesFacade salesUpFacade,
        CancellationToken cancellationToken)
    {
        var orders = await salesUpFacade.GetOrdersAsync(cancellationToken);

        return Results.Ok(orders);
    }
}