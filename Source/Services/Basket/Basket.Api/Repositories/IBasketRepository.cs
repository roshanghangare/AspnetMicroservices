using System.Threading.Tasks;
using Basket.Api.Entities;

namespace Basket.Api.Repositories
{
	public interface IBasketRepository
	{
		Task<ShoppingCart> GetBasket(string userName);
		Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart);
		Task DeleteBasket(string userName);
	}
}
