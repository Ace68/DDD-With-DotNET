using BrewUp.Shared.ReadModel;
using BrewUp.Warehouses.Domain.DomainServices;
using BrewUp.Warehouses.Facade;
using BrewUp.Warehouses.Facade.Validators;
using BrewUp.Warehouses.ReadModel.Dtos;
using BrewUp.Warehouses.ReadModel.Queries;
using BrewUp.Warehouses.ReadModel.Services;
using BrewUp.Warehouses.SharedKernel.Commands;
using BrewUp.Warehouses.SharedKernel.Contracts;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace BrewUp.Api;

public static class WarehouseModule
{
    public static void RegisterWarehouseModule(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<SetAvailabilityValidator>();
        services.AddSingleton<ValidationHandler>();

        services
            .AddScoped<IBrewUpCommandHandler<CreateBeerAvailability>,
                CreateBeerAvailabilityHandler<CreateBeerAvailability>>();
        services.AddScoped<IBrewUpCommandHandler<UpdateAvailabilityDueToProductionOrder>,
            UpdateBeerAvailabilityHandler>();

        services.AddScoped<IWarehousesFacade, WarehousesFacade>();
        services.AddScoped<IAvailabilityService, AvailabilityService>();
        services.AddScoped<IQueries<Availability>, AvailabilityQueries>();
    }
    
    public static void ConfigureWarehouseEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/v1/wareHouses/")
            .WithTags("Warehouses");

        group.MapPost("/availabilities", HandleSetAvailabilities)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("SetAvailabilities");
        group.MapGet("/availabilities", HandleGetAvailabilities)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("GetAvailabilities");
    }
    
    private static async Task<IResult> HandleSetAvailabilities(
        IWarehousesFacade warehousesFacade,
        IValidator<SetAvailabilityJson> validator,
        ValidationHandler validationHandler,
        SetAvailabilityJson body,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        await validationHandler.ValidateAsync(validator, body);
        if (!validationHandler.IsValid)
            return Results.BadRequest(validationHandler.Errors);
		
        await warehousesFacade.SetAvailabilityAsync(body, cancellationToken);

        return Results.Ok();
    }
	
    private static async Task<IResult> HandleGetAvailabilities(
        IWarehousesFacade warehousesFacade,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
		
        var availabilities = await warehousesFacade.GetAvailabilitiesAsync(cancellationToken);

        return Results.Ok(availabilities);
    }
}