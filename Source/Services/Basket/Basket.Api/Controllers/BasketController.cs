using System;
using System.Net;
using System.Threading.Tasks;
using Basket.Api.Entities;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class BasketController : ControllerBase
	{
		readonly IBasketRepository _basketRepository;

		public BasketController(IBasketRepository basketRepository)
		{
			_basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
		}

		[HttpGet("{userName}", Name = "GetBasket")]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
		{
			return Ok(await _basketRepository.GetBasket(userName) ?? new ShoppingCart(userName));
		}

		[HttpPost]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
		{
			return Ok(await _basketRepository.UpdateBasket(shoppingCart));
		}

		[HttpGet("{userName}", Name = "DeleteBasket")]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteBasket(string userName)
		{
			await _basketRepository.DeleteBasket(userName);
			return Ok();
		}
	}
}
