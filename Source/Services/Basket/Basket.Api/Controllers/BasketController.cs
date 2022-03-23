using System;
using System.Net;
using System.Threading.Tasks;
using Basket.Api.Entities;
using Basket.Api.GrpcServiceClients;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class BasketController : ControllerBase
	{
		readonly IBasketRepository basketRepository;
		readonly DiscountGrpcService discountGrpcService;

		public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
		{
			this.basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
			this.discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
		}

		[HttpGet("{userName}", Name = "GetBasket")]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
		{
			return Ok(await basketRepository.GetBasket(userName) ?? new ShoppingCart(userName));
		}

		[HttpPost]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
		{
			// TODO : Communicate with Discount.Grpc and
			// calculate latest prices of product into shopping cart

			foreach (var item in shoppingCart.Items)
			{
				var coupon = await discountGrpcService.GetDiscount(item.ProductName);
				item.Price -= coupon.Amount;
			}
			return Ok(await basketRepository.UpdateBasket(shoppingCart));
		}

		[HttpDelete("{userName}", Name = "DeleteBasket")]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteBasket(string userName)
		{
			await basketRepository.DeleteBasket(userName);
			return Ok();
		}
	}
}
