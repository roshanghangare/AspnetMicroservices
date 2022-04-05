using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
	public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
	{
		readonly IOrderRepository orderRepository;
		readonly IMapper mapper;
		readonly ILogger<UpdateOrderCommandHandler> logger;

		public DeleteOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper,
			ILogger<UpdateOrderCommandHandler> logger)
		{
			this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
		{
			var orderToDelete = await orderRepository.GetByIdAsync(request.Id);

			if (orderToDelete == null)
				throw new NotFoundException(nameof(Order), request.Id);
			await orderRepository.DeleteAsync(orderToDelete);

			logger.LogInformation($"Order {orderToDelete.Id} is successfully deleted.");

			return Unit.Value; ;
		}
	}
}
