﻿using BrewUp.Shared.DomainIds;
using Muflone.Messages.Events;

namespace BrewUp.Sales.SharedKernel.Events;

public sealed class SalesOrderClosed(SalesOrderId aggregateId, Guid correlationId) : DomainEvent(aggregateId, correlationId)
{
	public readonly SalesOrderId SalesOrderId = aggregateId;
}