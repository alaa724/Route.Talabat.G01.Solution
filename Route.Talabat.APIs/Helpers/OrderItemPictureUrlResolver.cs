﻿using AutoMapper;
using AutoMapper.Execution;
using AutoMapper.Internal;
using Route.Talabat.APIs.DTO;
using System.Linq.Expressions;
using System.Reflection;
using Talabat.Core.Entities.Order_Aggregate;

namespace Route.Talabat.APIs.Helpers
{
	public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
	{
		private readonly IConfiguration _configuration;

		public OrderItemPictureUrlResolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.Product.ProductUrl))
				return $"{_configuration["ApiBaseUrl"]}/{source.Product.ProductUrl}";

			return string.Empty;
		}
	}
}
