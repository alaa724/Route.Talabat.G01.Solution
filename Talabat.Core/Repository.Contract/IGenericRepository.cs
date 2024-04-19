
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repository.Contract
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		Task<IReadOnlyList<T>> GetAllAsync();

		Task<T?> GetAsync(int id);

		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec );

		Task<T?> GetWithSpecAsync(ISpecification<T> spec /*, int id*/);
	}
}
