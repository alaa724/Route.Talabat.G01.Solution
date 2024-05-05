using Microsoft.EntityFrameworkCore;
using System;
using Talabat.Core.Entities;
using Talabat.Core.Repository.Contract;
using Talabat.Core.Specifications;
using Talabat.Infrastructure.Data;

namespace Talabat.Infrastructure
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreDbContext _dbContext;

		public GenericRepository(StoreDbContext dbContext) // Ask CLR for creating object from dbContext implicitly
		{
			_dbContext = dbContext;
		}

		public void Add(T entity)
			=> _dbContext.Set<T>().Add(entity);


		public void Update(T entity)
			=> _dbContext.Set<T>().Update(entity);

		public void Delete(T entity)
			=> _dbContext.Set<T>().Remove(entity);

		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			//if (typeof(T) == typeof(Product))
			//	return (IEnumerable<T>)await _dbContext.Set<Product>().Include(P => P.Brand).Include(P => P.Category).AsNoTracking().ToListAsync();
			return await _dbContext.Set<T>().ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
		{
			return await ApplySpecifications(spec).AsNoTracking().ToListAsync();
		}

		public async Task<T?> GetAsync(int id)
		{
			//if (typeof(T) == typeof(Product))
			//	return await _dbContext.Set<Product>().Where(P => P.Id == id).Include(P => P.Brand)
			//	 .Include(P => P.Category).AsNoTracking().FirstOrDefaultAsync() as T;
			return await _dbContext.Set<T>().FindAsync(id);
		}

		public async Task<int> GetCountAsync(ISpecification<T> spec)
		{
			return await ApplySpecifications(spec).CountAsync();
		}

		public async Task<T?> GetWithSpecAsync(ISpecification<T> spec/*, int id*/)
		{
			return await ApplySpecifications(spec).AsNoTracking().FirstOrDefaultAsync();
		}

		private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
		{
			return SpecificationsEvaluter<T>.GetQuery(_dbContext.Set<T>(), spec);
		}
	}
}
