﻿using BrewUp.Infrastructure.RabbitMq;
using BrewUp.Warehouses.Infrastructures.RabbitMq.Commands;
using BrewUp.Warehouses.Infrastructures.RabbitMq.Events;
using BrewUp.Warehouses.ReadModel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Persistence;
using Muflone.Transport.RabbitMQ;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Factories;
using Muflone.Transport.RabbitMQ.Models;

namespace BrewUp.Warehouses.Infrastructures.RabbitMq;

public static class RabbitMqHelper
{
	public static IServiceCollection AddRabbitMqForWarehousesModule(this IServiceCollection services,
		RabbitMqSettings rabbitMqSettings)
	{
		var serviceProvider = services.BuildServiceProvider();
		var repository = serviceProvider.GetRequiredService<IRepository>();
		var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

		var rabbitMqConfiguration = new RabbitMQConfiguration(rabbitMqSettings.Host, rabbitMqSettings.Username,
			rabbitMqSettings.Password, rabbitMqSettings.ExchangeCommandName, rabbitMqSettings.ExchangeEventName,
			rabbitMqSettings.ClientId);
		var connectionFactory = new RabbitMQConnectionFactory(rabbitMqConfiguration, loggerFactory);

		services.AddMufloneTransportRabbitMQ(loggerFactory, rabbitMqConfiguration);

		serviceProvider = services.BuildServiceProvider();
		var consumers = serviceProvider.GetRequiredService<IEnumerable<IConsumer>>();
		consumers = consumers.Concat(new List<IConsumer>
		{
			new CreateBeerAvailabilityConsumer(repository,
				connectionFactory,
				loggerFactory),
			new BeerAvailabilityCreatedConsumer(serviceProvider.GetRequiredService<IAvailabilityService>(),
				connectionFactory,
				loggerFactory),
			
			new UpdateAvailabilityDueToProductionOrderConsumer(repository,
				connectionFactory,
				loggerFactory),
			new AvailabilityUpdatedDueToProductionOrderConsumer(serviceProvider.GetRequiredService<IAvailabilityService>(),
				serviceProvider.GetRequiredService<IEventBus>(),
				connectionFactory, loggerFactory),
			
			new AskForAvailabilityConsumer(repository,
				connectionFactory,
				loggerFactory),
			new BeerAvailableConsumer(serviceProvider.GetRequiredService<IEventBus>(),
				connectionFactory,
				loggerFactory),
			new BeerNotAvailableConsumer(serviceProvider.GetRequiredService<IEventBus>(),
				connectionFactory,
				loggerFactory)
		});
		services.AddMufloneRabbitMQConsumers(consumers);

		return services;
	}
}