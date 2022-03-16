using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
	public interface IProductRepository
	{
		Task<IEnumerable<Product>> GetProducts();
		Task<IEnumerable<Product>> GetProductsByCategory(string categoryName);
		Task<IEnumerable<Product>> GetProductByName(string name);
		Task<Product> GetProduct(string id);
		Task CreateProduct(Product product);
		Task<bool> UpdateProduct(Product product);
		Task<bool> DeleteProduct(string id);
	}
}
 