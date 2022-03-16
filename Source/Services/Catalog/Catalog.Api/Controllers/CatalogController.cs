using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class CatalogController : ControllerBase
	{
		readonly IProductRepository _repository;
		readonly ILogger<CatalogController> _logger;

		public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await _repository.GetProducts();
			return Ok(products);
		}

		[HttpGet("{id}:length(24)", Name = "GetProduct")]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductById(string id)
		{
			var product = await _repository.GetProduct(id);
			if (product != null) return Ok(product);
			_logger.LogError($"Product with id {id} not found");
			return NotFound();
		}

		[Route("[action]/{category}", Name = "GetProductByCategory")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
		{
			var product = await _repository.GetProductsByCategory(category);
			if (product != null) return Ok(product);
			_logger.LogError($"Product with id {category} not found");
			return NotFound();
		}

		[HttpPost]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
		{
			await _repository.CreateProduct(product);
			return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
		}

		[HttpPut]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> UpdateProduct([FromBody] Product product)
		{
			return Ok(await _repository.UpdateProduct(product));
		}

		[HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteProductById(string id)
		{
			return Ok(await _repository.DeleteProduct(id));
		}
	}
}
