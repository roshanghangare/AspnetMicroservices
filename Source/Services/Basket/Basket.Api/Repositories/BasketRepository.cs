using System;
using System.Threading.Tasks;
using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Api.Repositories
{
	public class BasketRepository: IBasketRepository
	{
		readonly IDistributedCache _redisCache;

		public BasketRepository(IDistributedCache redisCache)
		{
			_redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
		}

		public async Task<ShoppingCart> GetBasket(string userName)
		{
			var basket = await _redisCache.GetStringAsync(userName);
			
			return string.IsNullOrWhiteSpace(basket) ? null : 
				JsonConvert.DeserializeObject<ShoppingCart>(basket);
		}

		public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
		{
			await _redisCache.SetStringAsync(shoppingCart.UserName, JsonConvert.SerializeObject(shoppingCart));
			return await GetBasket(shoppingCart.UserName);
		}

		public async Task DeleteBasket(string userName)
		{
			await _redisCache.RemoveAsync(userName);
		}
	}
}
