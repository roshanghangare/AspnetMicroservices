using System;
using System.Threading.Tasks;
using Discount.Grpc.Protos;

namespace Basket.Api.GrpcServiceClients
{
	public class DiscountGrpcService
	{
		readonly DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient;

		public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
		{
			this.discountProtoServiceClient =
				discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
		}

		public async Task<CouponModel> GetDiscount(string productName)
		{
			var discountRequest = new GetDiscountRequest {ProductName = productName};
			return await discountProtoServiceClient.GetDiscountAsync(discountRequest);
		}

	}
}
