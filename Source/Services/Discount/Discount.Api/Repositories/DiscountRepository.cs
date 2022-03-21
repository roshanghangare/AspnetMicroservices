﻿using System;
using System.Threading.Tasks;
using Dapper;
using Discount.Api.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Api.Repositories
{
	public class DiscountRepository : IDiscountRepository
	{
		readonly IConfiguration configuration;
		public DiscountRepository(IConfiguration configuration)
		{
			this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}
		public async Task<Coupon> GetDiscount(string productName)
		{
			await using var connection =
				new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
				("SELECT * FROM Coupon WHERE ProductName = @ProductName", 
					new { ProductName = productName });

			return coupon ?? new Coupon
			{
				ProductName = "No Discount",
				Amount = 0,
				Description = "No discount desc"
			};
		}

		public async Task<bool> CreateDiscount(Coupon coupon)
		{
			await using var connection = new NpgsqlConnection
				(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
				await connection.ExecuteAsync
				("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
					new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

			return affected != 0;
		}

		public async Task<bool> UpdateDiscount(Coupon coupon)
		{
			await using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected = await connection.ExecuteAsync
			("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
				new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

			return affected != 0;
		}

		public async Task<bool> DeleteDiscount(string productName)
		{
			await using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
				new { ProductName = productName });

			return affected != 0;
		}
	}
}
