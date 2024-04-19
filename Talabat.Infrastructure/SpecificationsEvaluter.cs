using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Infrastructure
{
	internal static class SpecificationsEvaluter<TEntity> where TEntity : BaseEntity
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecification<TEntity> spec)
		{
			var query = inputQuery;

			if (spec.Criteria is not null)
				query = query.Where(spec.Criteria);

			// query = _dbContext.Set<Product>().Where(P => P.Id == id)
			// Includes 
			// 1. P => P.Brand
			// 2. P => P.Category

			query = spec.Includes.Aggregate(query, (currentquery, includeExpression) => currentquery.Include(includeExpression));
			//currentquery -> query = _dbContext.Set<Product>().Where(P => P.Id == id)
			//1-> query = _dbContext.Set<Product>().Where(P => P.Id == id).Include(P => P.Brand)
			//2-> query = _dbContext.Set<Product>().Where(P => P.Id == id).Include(P => P.Brand).Include(P => P.Category)

			return query;


		}
	}
}
