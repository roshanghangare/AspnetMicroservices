﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
	public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
	{
		readonly IOrderRepository orderRepository;
		readonly IMapper mapper;
		readonly ILogger<UpdateOrderCommandHandler> logger;

		public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper,
			ILogger<UpdateOrderCommandHandler> logger)
		{
			this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
		{
			var orderToUpdate = await orderRepository.GetByIdAsync(request.Id);
			if (orderToUpdate == null)
				logger.LogError("Order does not exist");

			mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));

			await orderRepository.UpdateAsync(orderToUpdate);

			logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated.");

			return Unit.Value;;
		}
	}
}
