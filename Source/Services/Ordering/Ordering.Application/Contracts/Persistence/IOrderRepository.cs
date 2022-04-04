using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.Domain.Entities;

namespace Ordering.Application.Contracts.Persistence
{
	interface IOrderRepository : IAsyncRepository<Order>
	{
		Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
	}
}
